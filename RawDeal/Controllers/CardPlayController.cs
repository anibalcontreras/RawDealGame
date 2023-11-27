using RawDealView;
using RawDeal.Models;
using RawDeal.Exceptions;
using RawDeal.Models.Effects;
using RawDeal.Observer;
using RawDeal.Models.Reversals;
using RawDeal.Utilities;
using RawDealView.Options;

namespace RawDeal.Controllers;

public class CardPlayController
{
    private readonly View _view;
    private readonly EffectCatalog _effectCatalog;
    private readonly List<IObserver> _observers = new List<IObserver>();
    private readonly EventManager _eventManager;
    private readonly ReversalCatalog _reversalCatalog;
    private readonly PlayerActionsController _playerActionsController;
    private Player CurrentPlayer { get; set; }
    private Player Opponent { get; set; }
    private List<Card> PlayableCards { get; set; }
    private int SelectedPlayIndex { get; set; }
    private int SelectedReversalIndex { get; set; }
public CardPlayController(View view)
    {
        _view = view;
        _effectCatalog = new EffectCatalog(view);
        _eventManager = EventManager.Instance;
        _reversalCatalog = new ReversalCatalog(view);
        _playerActionsController = new PlayerActionsController(view);
    }
    public void HandlePlayCardAction(Player firstPlayer, Player secondPlayer)
    {
        InitializePlayers(firstPlayer, secondPlayer);
        if (AttemptToPlayCard())
            TryingExecuteCardPlay();
    }
    private void InitializePlayers(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;
        PlayableCards = PlayUtility.GetPlayableCards(CurrentPlayer.GetHand(), CurrentPlayer.Fortitude);
    }
    private bool AttemptToPlayCard()
    {
        List<string> formatPlayableCards=
            PlayUtility.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude);
        SelectedPlayIndex = _view.AskUserToSelectAPlay(formatPlayableCards);
        return SelectedPlayIndex != -1;
    }
    private Card GetPlayedCard()
    {
        try
        {
            Card card = (Card)GetSelectedPlay().CardInfo;
            return card;
        }
        catch (InvalidCastException)
        {
            throw new InvalidCardConversionException();
        }
    }
    private string GetPlayedAs()
    {
        return GetSelectedPlay().PlayedAs;
    }
    private Play GetSelectedPlay()
    {
        List<Card> playerHand = CurrentPlayer.GetHand();
        List<Play> playablePlays = PlayUtility.GetPlayablePlays(playerHand, CurrentPlayer.Fortitude);
        return playablePlays[SelectedPlayIndex];
    }

    private void ApplyCardEffectDueReversal(Card reversalCard)
    {
        string playedAs = GetPlayedAs();
        Effect cardEffect = _effectCatalog.GetEffectBy(reversalCard.Title, playedAs);
        bool hasLost = cardEffect.Apply(Opponent, CurrentPlayer, reversalCard);
        if (hasLost)
            EndGame(Opponent);
    }
    
    private void ApplyCardEffect()
    {
        Card playedCard = GetPlayedCard();
        string playedAs = GetPlayedAs();
        Effect cardEffect = _effectCatalog.GetEffectBy(playedCard.Title, playedAs);
        bool hasLost = cardEffect.Apply(CurrentPlayer, Opponent, GetPlayedCard());
        if (hasLost)
            EndGame(CurrentPlayer);
    }
    private void TryingExecuteCardPlay()
    {
        AnnounceAttemptToPlayCard();
        if (CheckIfIsPossibleToUseAReversal())
            _eventManager.Notify("CardReversedByHand", "CardReversedByHand", Opponent);
        else
        {
            AnnounceSuccessfulCardPlay();
            ApplyCardEffect();
        }
    }
    
    private bool CheckIfIsPossibleToUseAReversal()
    {
        Card playedCard = GetPlayedCard();
        playedCard.SetPlayedAs(GetPlayedAs());
        List<Card> reversalsListCards = CanReverseDamageByHand(Opponent, playedCard);
        List<String> formattedReversalCards = PlayUtility.GetFormattedReversalCards(reversalsListCards);
        SelectedReversal(formattedReversalCards);
    
        if (SelectedReversalIndex != -1)
        {
            PerformReversalActions(playedCard, reversalsListCards[SelectedReversalIndex]);
            return true;
        }
        return false;
    }
    
    private void PerformReversalActions(Card playedCard, Card reversalCard)
    {
        AnnounceReversal(playedCard);
        int damageValue = ParseDamage(reversalCard);
        ApplyEffectsBasedOnDamage(reversalCard, damageValue, playedCard);
        RestoreOriginalDamageValue(reversalCard);
        CheckForEndGameCondition();
    }

    private void AnnounceReversal(Card playedCard)
    {
        List<Card> reversalsListCards = CanReverseDamageByHand(Opponent, playedCard);
        List<String> formattedReversalCards = PlayUtility.GetFormattedReversalCards(reversalsListCards);
        string opponentName = Opponent.Superstar.Name;
        _view.SayThatPlayerReversedTheCard(opponentName, formattedReversalCards[SelectedReversalIndex]);
    }
    
    private void ApplyEffectsBasedOnDamage(Card reversalCard, int damageValue, Card playedCard)
    {
        if (damageValue > 0)
        {
            ApplyCardEffectDueReversal(reversalCard);
            _playerActionsController.AddCardToRingsideDueReversedByHand(CurrentPlayer, playedCard);
        }
        else
        {
            _playerActionsController.AddCardToRingsideDueReversedByHand(CurrentPlayer, playedCard);
            _playerActionsController.RemoveCardFromHandDueToReversal(Opponent, reversalCard);  
        }
    }

    private void RestoreOriginalDamageValue(Card card)
    {
        card.TemporarilySetDamage(card.Damage);
    }

    private void CheckForEndGameCondition()
    {
        if (CurrentPlayer.HasEmptyArsenal())
            EndGame(Opponent);
    }
    
    private int ParseDamage(Card reversalCard)
    {
        if (int.TryParse(reversalCard.Damage, out int reversalDamageValue))
            return reversalDamageValue;
        throw new InvalidDamageValueException();
    }
    
    private void SelectedReversal(List<string> reversalCards)
    {
        string opponentName = Opponent.Superstar.Name;
        SelectedReversalIndex = _view.AskUserToSelectAReversal(opponentName, reversalCards);
    }
    
    private List<Card> CanReverseDamageByHand(Player player, Card playedCard)
    {
        List<Card> cardsInHand = player.GetHand();
        List<Card> possibleReversals = new List<Card>();

        foreach (Card cardInHand in cardsInHand)
        {
            if (IsPossibleToUseReversal(cardInHand))
            {
                Reversal reversalCard = _reversalCatalog.GetReversalBy(cardInHand.Title);
                if (reversalCard.CanReverseFromHand(playedCard, cardInHand, player))
                    possibleReversals.Add(cardInHand);
            }
        }
        return possibleReversals;
    }

    private bool IsPossibleToUseReversal(Card cardInHand)
    {
        try
        {
            _reversalCatalog.GetReversalBy(cardInHand.Title);
            return true;
        }
        catch (ReversalNotFoundException)
        {
            return false;
        }
    }
    
    private void AnnounceAttemptToPlayCard()
    {
        string selectedPlay =
            PlayUtility.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude)[SelectedPlayIndex];
        _view.SayThatPlayerIsTryingToPlayThisCard(CurrentPlayer.Superstar.Name, selectedPlay);
    }
    
    private void AnnounceSuccessfulCardPlay()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
    
    private void EndGame(Player winningPlayer)
    {
        _eventManager.Notify("EndGame", "EndGame", winningPlayer);
    }
}