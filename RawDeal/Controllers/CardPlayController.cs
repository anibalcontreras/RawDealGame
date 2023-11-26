using RawDealView;
using RawDeal.Models;
using RawDeal.Exceptions;
using RawDeal.Models.Effects;
using RawDeal.Interfaces;
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

    private bool IsEffectActive { get; set; } = false;
    private SelectedEffect CurrentSelectedEffect { get; set; }
    
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
        Console.WriteLine("El ringarea del player es " + CurrentPlayer.GetRingArea().Count);
        bool hasLost = cardEffect.Apply(CurrentPlayer, Opponent, GetPlayedCard());
        if (hasLost)
            EndGame(CurrentPlayer);
    }
    private void TryingExecuteCardPlay()
    {
        AnnounceAttemptToPlayCard();
        ConditionsDueJockeyingForPosition();
        if (CheckIfIsPossibleToUseAReversal())
        {
            _eventManager.Notify("CardReversedByHand", "CardReversedByHand", Opponent);
        }
        else
        {
            AnnounceSuccessfulCardPlay();
            CheckIfThePlayedCardIsJockeyingForPosition();
            ApplyCardEffect();
            
        }
    }

    private void CheckIfThePlayedCardIsJockeyingForPosition()
    {
        Card playedCard = GetPlayedCard();
        if (playedCard.Title == "Jockeying for Position")
        {
            SelectedEffect selectedEffect = _view.AskUserToSelectAnEffectForJockeyForPosition(CurrentPlayer.Superstar.Name);
            CurrentSelectedEffect = selectedEffect;
            IsEffectActive = true;
        }
    }

    private void ConditionsDueJockeyingForPosition()
    {
        if (IsEffectActive)
        {
            switch (CurrentSelectedEffect)
            {
                case SelectedEffect.NextGrappleIsPlus4D:
                    Card playedCard = GetPlayedCard();
                    if (playedCard.Subtypes.Contains("Grapple"))
                    {
                        playedCard.IncrementDamage(int.Parse(playedCard.Damage), 4);
                    }
                    break;
            }
        }
    }

    public void ReturnToNormalState(Card card)
    {
        Console.WriteLine("llegamos a este punto");
        Console.WriteLine("Cuanto vale este selected effect? " + CurrentSelectedEffect);
        if (IsEffectActive)
            
            switch (CurrentSelectedEffect)
            {
                case SelectedEffect.NextGrappleIsPlus4D:
                    Console.WriteLine("La played card es: " + card.Title);
                    if (card.Subtypes.Contains("Grapple"))
                    {
                        Console.WriteLine("ACA SIONO");
                        card.DecrementDamage(int.Parse(card.Damage), 4);
                    }
                    break;
            }
    }

    private void InstantiateTheNormalEffectState()
    {
        IsEffectActive = false;
    }
    
    private bool CheckIfIsPossibleToUseAReversal()
    {
        Card playedCard = GetPlayedCard();
        Console.WriteLine("el damage de este playedCard es " + playedCard.Damage);
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
        List<Card> reversalsListCards = CanReverseDamageByHand(Opponent, playedCard);
        List<String> formattedReversalCards = PlayUtility.GetFormattedReversalCards(reversalsListCards);
        _view.SayThatPlayerReversedTheCard(Opponent.Superstar.Name, formattedReversalCards[SelectedReversalIndex]);

        // Guarda el daño original en caso de que sea necesario revertirlo después de aplicar los efectos.
        string originalDamage = reversalCard.Damage;
        try
        {
            int damageValue = ParseDamage(reversalCard, playedCard);
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
        finally
        {
            // Restablece el daño de la carta de reversión a su valor original, que es "#".
            reversalCard.TemporarilySetDamage(originalDamage);
        }

        if (CurrentPlayer.HasEmptyArsenal())
        {
            EndGame(Opponent);
        }
    }
    
    private int ParseDamage(Card reversalCard, Card playedCard)
    {
        if (reversalCard.Damage == "#")
        {
            reversalCard.TemporarilySetDamage(playedCard.Damage);
            return int.Parse(playedCard.Damage);
        }
        else if (int.TryParse(reversalCard.Damage, out int reversalDamageValue))
        {
            return reversalDamageValue;
        }
        else
        {
            throw new InvalidDamageValueException();
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
            try
            {
                if (IsPossibleToUseReversal(cardInHand))
                {
                    Reversal reversalCard = _reversalCatalog.GetReversalBy(cardInHand.Title);
                    if (reversalCard.CanReverseFromHand(playedCard, cardInHand, player))
                    {
                        possibleReversals.Add(cardInHand);
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                continue;
            }
        }
        return possibleReversals;
    }
    
    private bool IsPossibleToUseReversal(Card cardInHand)
    {
        try
        {
            Reversal potentialReversal = _reversalCatalog.GetReversalBy(cardInHand.Title);
            return true;
        }
        catch (KeyNotFoundException)
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