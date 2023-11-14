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
    
    private void CanReverseDamageByDeck(Player player, Card lastCard, Card playedCard, Player opponent, int i)
    {
        if (IsPossibleToUseReversal(lastCard))
        {
            Reversal reversalCard = _reversalCatalog.GetReversalBy(lastCard.Title);
            if (reversalCard.CanReverseFromDeck(lastCard, player, playedCard, opponent, i))
            {
                Console.WriteLine(player.GetArsenal().Count);
                if (player.GetArsenal().Count == 0)
                {
                    Console.WriteLine("Estas aca 1");
                    _eventManager.Notify("EndGame", "EndGame", opponent);
                }
                else
                {
                    _eventManager.Notify("CardReversedByDeck", "CardReversedByDeck", player);    
                }
                if (int.Parse(playedCard.StunValue) > 0 && (int.Parse(playedCard.Damage) - 1) > i)
                {
                    int selectedNumberOfCards = _view.AskHowManyCardsToDrawBecauseOfStunValue(opponent.Superstar.Name, int.Parse(playedCard.StunValue));
                    DrawCardDueToStunValue(opponent, selectedNumberOfCards);
                }
                throw new CardReversedButGameContinuesException();
            }
        }
    }
    
    private bool IsPossibleToUseReversal(Card reversalCard)
    {
        Reversal potentialReversal = _reversalCatalog.GetReversalBy(reversalCard.Title);
        return potentialReversal != null;
    }

    public void ApplyDamage(Player player, int cardIndex)
    {
        List<Card> hand = player.GetHand();
        List<Card> ringArea = player.GetRingArea();

        Card cardToApply = hand[cardIndex];
        hand.RemoveAt(cardIndex);
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