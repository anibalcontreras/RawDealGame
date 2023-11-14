using RawDealView;
using RawDeal.Models;
using RawDeal.Exceptions;
using RawDeal.Models.Effects;
using RawDeal.Interfaces;
using RawDeal.Models.Reversals;
using RawDeal.Utilities;
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
        _eventManager = EventManager.GetInstance();
        _reversalCatalog = new ReversalCatalog(view);
        _playerActionsController = new PlayerActionsController(view);
    }
    public void HandlePlayCardAction(Player firstPlayer, Player secondPlayer)
    {
        InitializePlayers(firstPlayer, secondPlayer);
        if (AttemptToPlayCard())
        {
            TryingExecuteCardPlay();
        }
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
        List<Play> playablePlays = PlayUtility.GetPlayablePlays(CurrentPlayer.GetHand(), CurrentPlayer.Fortitude);
        return playablePlays[SelectedPlayIndex];
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
        try
        {
            CheckIfIsPossibleToUseAReversal();
            AnnounceSuccessfulCardPlay();
            ApplyCardEffect();
        } catch (CardReversedButGameContinuesException)
        {
            _eventManager.Notify("CardReversedByHand", "CardReversedByHand", Opponent);
        }
    }
    
    private void CheckIfIsPossibleToUseAReversal()
    {
        Card playedCard = GetPlayedCard();
        playedCard.SetPlayedAs(GetPlayedAs());
        List<Card> reversalsListCards = CanReverseDamageByHand(Opponent, playedCard);
        List<String> formattedReversalCards = PlayUtility.GetFormattedReversalCards(reversalsListCards);
        SelectedReversal(formattedReversalCards);
        if (SelectedReversalIndex != -1)
        {
            Card reversalCard = reversalsListCards[SelectedReversalIndex];
            
            _view.SayThatPlayerReversedTheCard(Opponent.Superstar.Name, formattedReversalCards[SelectedReversalIndex]);
            
            Console.WriteLine("Se agrega de " + CurrentPlayer.Superstar.Name + " la carta " + playedCard.Title);
            _playerActionsController.AddCardToRingsideDueReversedByHand(CurrentPlayer, playedCard);
            
            Console.WriteLine("Se remueve de " + Opponent.Superstar.Name + " la carta " + reversalCard.Title);
            _playerActionsController.RemoveCardFromHandDueToReversal(Opponent, reversalCard);
            
            if (CurrentPlayer.HasEmptyArsenal())
            {
                EndGame(Opponent);
            }
            throw new CardReversedButGameContinuesException();
        }
    }
    
    
    private bool SelectedReversal(List<string> reversalCards)
    {
        SelectedReversalIndex = _view.AskUserToSelectAReversal(Opponent.Superstar.Name, reversalCards);
        return SelectedReversalIndex != -1;
    }
    
    private List<Card> CanReverseDamageByHand(Player player, Card playedCard)
    {

        List<Card> cardsInHand = player.GetHand();
        List<Card> possibleReversals = new List<Card>();

        foreach (Card cardInHand in cardsInHand)
        {
            if (IsPossibleToUseReversal(cardInHand))
            {
                Reversal reversalCard = _reversalCatalog.GetReversalBy(playedCard.Title);
                if (reversalCard.CanReverseFromHand(playedCard, cardInHand, player))
                {
                    possibleReversals.Add(cardInHand);
                }
                
            }
        }
        return possibleReversals;
    }
    
    
    private bool IsPossibleToUseReversal(Card cardInHand)
    {
        Reversal potentialReversal = _reversalCatalog.GetReversalBy(cardInHand.Title);
        return potentialReversal != null;
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