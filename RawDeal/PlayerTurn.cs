using RawDeal.Interfaces;
using RawDeal.Models.Effects;
using RawDealView;
using RawDealView.Options;
using RawDeal.Models;
using RawDeal.Models.Reversals;

namespace RawDeal;
public class PlayerTurn : IObserver
{
    public void Update(string message)
    {
        if (message == "CardReversedByDeck")
        {
            Console.WriteLine("NOS LLEGO EL MENSAJE DE QUE SE REVERSÓ UNA CARTA DESDE EL MAZO");
            continueTurn = false;
            ResetAbilityUsage();
        }
        else if (message == "CardReversedByHand")
        {
            Console.WriteLine("NOS LLEGO EL MENSAJE DE QUE SE REVERSÓ UNA CARTA DESDE LA MANO");
            continueTurn = false;
            ResetAbilityUsage();
        }
    }

    private Card _selectedCard;
    private bool continueTurn = true;
    private readonly View _view;
    private Player CurrentPlayer { get; set; }
    private Player Opponent { get; set; }
    private List<Card> PlayableCards { get; set; }
    private int SelectedPlayIndex { get; set; }
    private int SelectedReversalIndex { get; set; }
    private bool GameOn { get; set; } = true;
    private bool TurnOn { get; set; } = true;
    private EffectCatalog _effectCatalog;
    // private ReversalCatalog _reversalCatalog;

    public PlayerTurn(View view)
    {
        _view = view;
        _effectCatalog = new EffectCatalog(view);
        // _reversalCatalog = new ReversalCatalog(view);
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
        player.DrawCard();
        _view.SayThatATurnBegins(player.Superstar.Name);
    }
    private void ExecutePlayerActions(Player firstPlayer, Player secondPlayer)
    {
        while (continueTurn)
        {
            continueTurn = HandleTurnActions(firstPlayer, secondPlayer);
        }
        continueTurn = true;
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
                if (!GameOn || !continueTurn) return false;
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
        return continueTurn;
    }
    
    private void HandlePlayCardAction(Player firstPlayer, Player secondPlayer)
    {
        InitializePlayers(firstPlayer, secondPlayer);
        if (AttemptToPlayCard())
        {
            ExecuteCardPlay(GetPlayedCard(), GetPlayedAs());
        }
    }

    private void InitializePlayers(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;
        PlayableCards = Play.GetPlayableCards(CurrentPlayer.GetHand(), CurrentPlayer.Fortitude);
    }

    private bool AttemptToPlayCard()
    {
        SelectedPlayIndex = _view.AskUserToSelectAPlay(Play.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude));
        return SelectedPlayIndex != -1;
    }

    private Card GetPlayedCard()
    {
        return GetSelectedPlay().CardInfo as Card;
    }

    private string GetPlayedAs()
    {
        return GetSelectedPlay().PlayedAs;
    }

    private Play GetSelectedPlay()
    {
        List<Play> playablePlays = Play.GetPlayablePlays(CurrentPlayer.GetHand(), CurrentPlayer.Fortitude);
        return playablePlays[SelectedPlayIndex];
    }

    private void ApplyCardSpecialEffect()
    {
        Effect cardEffect = _effectCatalog.GetEffectBy(GetPlayedCard().Title, GetPlayedAs());
        bool hasLost = cardEffect.Apply(CurrentPlayer, Opponent, GetPlayedCard());
        if (hasLost)
            EndGame(CurrentPlayer);
    }

    private void ExecuteCardPlay(Card playedCard, string playedAs)
    {
        AnnounceAttemptToPlayCard();
        if (CanBeReversed(playedCard, playedAs))
        {
            if (UserChoseNotToReverse())
            {
                AnnounceSuccessfulCardPlay();
                ApplyCardSpecialEffect();
            }
            else
            {
                HandleReversal(playedCard, playedAs);
            }
        }
        else
        {
            AnnounceSuccessfulCardPlay();
            ApplyCardSpecialEffect();   
        }
    }

    private bool UserChoseNotToReverse()
    {
        return SelectedReversalIndex == -1;
    }

    private void HandleReversal(Card playedCard, string playedAs)
    {
        Play selectedReversal = GetSelectedReversal(playedCard, playedAs);
        Card playedReversal = selectedReversal.CardInfo as Card;
        CancelPlayerTurn(GetFormattedReversal(playedCard, playedAs), playedReversal);
    }

    private Play GetSelectedReversal(Card playedCard, string playedAs)
    {
        List<Play> playableReversals = Play.GetPlayablePlaysReversals(Opponent.GetHand(), playedCard, Opponent.Fortitude, playedAs);
        return playableReversals[SelectedReversalIndex];
    }

    private string GetFormattedReversal(Card playedCard, string playedAs)
    {
        return Play.GetFormattedPlayableReversals(Opponent.GetHand(), playedCard, Opponent.Fortitude, playedAs)[SelectedReversalIndex];
    }

    private void CancelPlayerTurn(string playedReversal, Card playedReversalCard)
    {
        Console.WriteLine("Reversamos carta desde la mano");
        _view.SayThatPlayerReversedTheCard(Opponent.Superstar.Name, playedReversal);
        Update("CardReversedByHand");
        Opponent.OpponentUseReversal(playedReversalCard);
        CurrentPlayer.OpponentUseReversal(playedReversalCard);
    }

    private void AnnounceAttemptToPlayCard()
    {
        string selectedPlay = Play.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude)[SelectedPlayIndex];
        _view.SayThatPlayerIsTryingToPlayThisCard(CurrentPlayer.Superstar.Name, selectedPlay);
    }

    private void AnnounceSuccessfulCardPlay()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private bool CanBeReversed(Card playedCard, string playedAs)
    {
        List<string> possibleReversals = Play.GetFormattedPlayableReversals(Opponent.GetHand(), playedCard, Opponent.Fortitude, playedAs);
        return possibleReversals.Count > 0;
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