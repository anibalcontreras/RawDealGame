using RawDeal.Exceptions;
using RawDeal.Interfaces;
using RawDeal.Models;
using RawDeal.Models.Reversals;
using RawDealView;
namespace RawDeal.Controllers;
public class PlayerActionsController
{
    private readonly View _view;
    private readonly ReversalCatalog _reversalCatalog;
    private readonly List <IObserver> _observers = new List<IObserver>();
    private readonly EventManager _eventManager;
    private Player CurrentPlayer { get; set; }
    private Player Opponent { get; set; }
    private readonly CardPlayController _cardPlayController;
    
    public PlayerActionsController(View view)
    {
        _view = view;
        _reversalCatalog = new ReversalCatalog(view);
        _eventManager = EventManager.GetInstance();
    }
    
    public void InitializePlayers(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;
    }
    public void DrawCard(Player player)
    {
        List<Card> arsenal = player.GetArsenal();
        List<Card> hand = player.GetHand();

        if (!arsenal.Any()) return;

        Card lastCard = arsenal.Last();
        arsenal.Remove(lastCard);
        hand.Add(lastCard);
    }

    private void DrawCardDueToStunValue(Player player, int stunValue)
    {
        List<Card> arsenal = player.GetArsenal();
        List<Card> hand = player.GetHand();
        
        if (!arsenal.Any()) return;
        
        for (int i = 0; i < stunValue; i++)
        {
            if (!arsenal.Any()) return;
            Card lastCard = arsenal.Last();
            arsenal.Remove(lastCard);
            hand.Add(lastCard);
        }
        
        _view.SayThatPlayerDrawCards(player.Superstar.Name, stunValue);
    }

    public bool ReceiveDamage(Player player, int damageAmount, Card playedCard, Player opponent)
    {
        List<Card> arsenal = player.GetArsenal();
        List<Card> ringside = player.GetRingside();
        
        for (int i = 0; i < damageAmount; i++)
        {
            if (!arsenal.Any()) return true;

            Card lastCard = arsenal.Last();
            arsenal.Remove(lastCard);
            ringside.Add(lastCard);
            _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
            CanReverseDamageByDeck(player, lastCard, playedCard, opponent, i);
        }
        return false;
    }
    
    private void CanReverseDamageByDeck(Player player, Card lastCard, Card playedCard, Player opponent, int index)
    {
        try
        {
            if (TryGetReversalForCard(lastCard, out Reversal reversalCard) && 
                reversalCard.CanReverseFromDeck(lastCard, player, playedCard))
            {
                ProcessReversal(player, opponent, playedCard, index);
            }
        }
        catch (KeyNotFoundException)
        {
        }
    }
    
    private bool TryGetReversalForCard(Card card, out Reversal reversal)
    {
        return _reversalCatalog.TryGetReversalBy(card.Title, out reversal);
    }


    private void ProcessReversal(Player player, Player opponent, Card playedCard, int index)
    {
        NotifyEndGameIfNeeded(player, opponent);
        NotifyCardReversal(player);
        HandleStunValue(playedCard, opponent, index);
        throw new CardReversedButGameContinuesException();
    }
    

    private void NotifyEndGameIfNeeded(Player player, Player opponent)
    {
        if (player.GetArsenal().Count == 0)
        {
            _eventManager.Notify("EndGame", "EndGame", opponent);
        }
    }

    private void NotifyCardReversal(Player player)
    {
        _eventManager.Notify("CardReversedByDeck", "CardReversedByDeck", player);
    }

    private void HandleStunValue(Card playedCard, Player opponent, int index)
    {
        if (int.TryParse(playedCard.StunValue, out int stunValue) && stunValue > 0 && 
            int.TryParse(playedCard.Damage, out int damage) && (damage - 1) > index)
        {
            int selectedNumberOfCards = _view.AskHowManyCardsToDrawBecauseOfStunValue(opponent.Superstar.Name, stunValue);
            DrawCardDueToStunValue(opponent, selectedNumberOfCards);
        }
    }


    public void ApplyDamage(Player player, int cardIndex)
    {
        List<Card> hand = player.GetHand();
        List<Card> ringArea = player.GetRingArea();
        Card cardToApply = hand[cardIndex];
        hand.RemoveAt(cardIndex);
        Console.WriteLine("Aqui el originalDamage es: " + cardToApply.OriginalDamage);
        Console.WriteLine("Aqui el Card que se añade a RingArea es: " + cardToApply.Title);
        Console.WriteLine("Por otro lado el damage es: " + cardToApply.Damage);
        ringArea.Add(cardToApply);
    }

    public void DiscardCard(Player player, Card cardToDiscard)
    {
        List<Card> hand = player.GetHand();

        if (hand.Any(card => card.Title == cardToDiscard.Title))
        {
            AddCardToRingside(player, cardToDiscard);
            RemoveCardFromHand(player, cardToDiscard);
            _view.SayThatPlayerMustDiscardThisCard(player.Superstar.Name, cardToDiscard.Title);
        }
    }

    private void AddCardToRingside(Player player, Card card)
    {
        List<Card> ringside = player.GetRingside();
        ringside.Add(card);
    }
    
    private void AddCardToRingArea(Player player, Card card)
    {
        List<Card> ringArea = player.GetRingArea();
        ringArea.Add(card);
    }

    private void RemoveCardFromHand(Player player, Card card)
    {
        List<Card> hand = player.GetHand();
        hand.Remove(card);
    }

    public void AddCardToRingsideDueReversedByHand(Player player, Card card)
    {
        AddCardToRingside(player, card);
        RemoveCardFromHand(player, card);
    }
    public void RemoveCardFromHandDueToReversal(Player player, Card card)
    {
        RemoveCardFromHand(player, card);
        AddCardToRingArea(player, card);
    }
    
}