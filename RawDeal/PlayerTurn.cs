using RawDeal.Controllers;
using RawDeal.Exceptions;
using RawDeal.Models.Effects;
using RawDealView;
using RawDealView.Options;
using RawDeal.Models;

namespace RawDeal;
public class PlayerTurn
{
    private bool _continueTurn = true;
    private readonly View _view;
    private Player CurrentPlayer { get; set; } = null!;
    private Player Opponent { get; set; } = null!;
    private List<Card> PlayableCards { get; set; } = null!;
    private int SelectedPlayIndex { get; set; }
    private bool GameOn { get; set; } = true;
    private bool TurnOn { get; set; } = true;
    private readonly EffectCatalog _effectCatalog;
    private readonly PlayerActionsController _playerActionsController;
    
    public PlayerTurn(View view)
    {
        _view = view;
        _effectCatalog = new EffectCatalog(view);
        _playerActionsController = new PlayerActionsController(view);
    }
    public bool PlayTurn(Player firstPlayer, Player secondPlayer)
    {
        StartPlayerTurn(firstPlayer);
        ActivateSuperstarsAbility(firstPlayer, secondPlayer);
        ExecutePlayerActions(firstPlayer, secondPlayer);
        return GameOn;
    }
    private void StartPlayerTurn(Player player)
    {
        _playerActionsController.DrawCard(player);
        _view.SayThatATurnBegins(player.Superstar.Name);
    }
    private void ExecutePlayerActions(Player firstPlayer, Player secondPlayer)
    {
        while (_continueTurn)
        {
            _continueTurn = HandleTurnActions(firstPlayer, secondPlayer);
        }
        _continueTurn = true;
    }
    private NextPlay DetermineIfSuperstarCanActivateHisAbility(Player player)
    {
        if (CanActivateAbility(player))
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
    
        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    private bool CanActivateAbility(Player player)
    {
        return !player.Superstar.HasUsedAbility &&
               player.Superstar.ActivationMoment == AbilityActivation.InMenu &&
               player.Superstar.CanUseAbility(player);
    }
    private void ActivateStartOfTurnAbility(Player firstPlayer, Player secondPlayer)
    {
        firstPlayer.Superstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.StartOfTurn);
    }
    private void ActivateAutomaticSuperstarAbility(Player firstPlayer, Player secondPlayer)
    {
        if (!firstPlayer.Superstar.HasUsedAbility &&
            firstPlayer.Superstar.ActivationMoment == AbilityActivation.Automatic)
            firstPlayer.Superstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.Automatic);
    }
    private void ActivateSuperstarsAbility(Player firstPlayer, Player secondPlayer)
    {
        ActivateAutomaticSuperstarAbility(firstPlayer, secondPlayer);
        ActivateStartOfTurnAbility(firstPlayer, secondPlayer);
    }
    private bool HandleTurnActions(Player firstPlayer, Player secondPlayer)
    {
        _view.ShowGameInfo(firstPlayer.PlayerInfo(), secondPlayer.PlayerInfo());
        
        NextPlay turnActionsSelections = DetermineIfSuperstarCanActivateHisAbility(firstPlayer);
        
        switch(turnActionsSelections)
        {
            case NextPlay.ShowCards:
                HandleShowCardsActions(firstPlayer, secondPlayer);
                break;
            case NextPlay.PlayCard:
                HandlePlayCardAction(firstPlayer, secondPlayer);
                if (!GameOn || !_continueTurn) return false;
                break;
            case NextPlay.UseAbility:
                firstPlayer.Superstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.InMenu);
                break;
            case NextPlay.EndTurn:
                HandleEndTurnAction(firstPlayer, secondPlayer);
                return false;
            case NextPlay.GiveUp:
                HandleGiveUpAction(secondPlayer);
                return false;
        }
        return _continueTurn;
    }
    
    private void HandlePlayCardAction(Player firstPlayer, Player secondPlayer)
    {
        InitializePlayers(firstPlayer, secondPlayer);
        if (AttemptToPlayCard())
        {
            ExecuteCardPlay();
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
        List<string> formatPlayableCards = PlayUtility.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude);
        
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
            throw new InvalidCardConversionException("Failed to convert CardInfo to Card.");
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
    
    private void ApplyCardSpecialEffect()
    {
        Card playedCard = GetPlayedCard();
        string playedAs = GetPlayedAs();
        
        Effect cardEffect = _effectCatalog.GetEffectBy(playedCard.Title, playedAs);
        Console.WriteLine("El efecto es: " + cardEffect.ToString());
        bool hasLost = cardEffect.Apply(CurrentPlayer, Opponent, GetPlayedCard());
        if (hasLost)
            EndGame(CurrentPlayer);
    }
    private void ExecuteCardPlay()
    {
        AnnounceAttemptToPlayCard();
        AnnounceSuccessfulCardPlay();
        ApplyCardSpecialEffect();
    }
    

    private void AnnounceAttemptToPlayCard()
    {
        string selectedPlay = PlayUtility.GetFormattedPlayableCards(PlayableCards,
            CurrentPlayer.Fortitude)[SelectedPlayIndex];
        _view.SayThatPlayerIsTryingToPlayThisCard(CurrentPlayer.Superstar.Name, selectedPlay);
    }

    private void AnnounceSuccessfulCardPlay()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
    
    private void HandleEndTurnAction(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;

        if (Opponent.HasEmptyArsenal())
            EndGame(CurrentPlayer);
        if (CurrentPlayer.HasEmptyArsenal())
            EndGame(Opponent);
        TurnOn = false;
        ResetAbilityUsage();
    }

    private void ResetAbilityUsage()
    {
        CurrentPlayer.Superstar.MarkAbilityAsUnused();
    }

    private void HandleGiveUpAction(Player opponentPlayer)
    {
        EndGame(opponentPlayer);
    }

    private void EndGame(Player winningPlayer)
    {
        TurnOn = false;
        GameOn = false;
        _view.CongratulateWinner(winningPlayer.Superstar.Name);
    }

    private void HandleShowCardsActions(Player firstPlayer, Player secondPlayer)
    {
        CardSet showCardsActionsSelection = _view.AskUserWhatSetOfCardsHeWantsToSee();
        
        switch(showCardsActionsSelection)
        {
            case CardSet.Hand:
                DisplayFormattedCards(firstPlayer, firstPlayer.GetHand());
                break;

            case CardSet.RingArea:
                DisplayFormattedCards(firstPlayer, firstPlayer.GetRingArea());
                break;

            case CardSet.RingsidePile:
                DisplayFormattedCards(firstPlayer, firstPlayer.GetRingside());
                break;

            case CardSet.OpponentsRingArea:
                DisplayFormattedCards(secondPlayer, secondPlayer.GetRingArea());
                break;

            case CardSet.OpponentsRingsidePile:
                DisplayFormattedCards(secondPlayer, secondPlayer.GetRingside());
                break;
        }
    }
    private void DisplayFormattedCards(Player player, List<Card> cardSet)
    {
        List<string> formattedCards = player.GetFormattedCardsInfo(cardSet);
        _view.ShowCards(formattedCards);
    }
}