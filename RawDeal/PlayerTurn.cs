namespace RawDeal;

using RawDealView;
using RawDealView.Options;
using RawDeal.Models;

public class PlayerTurn
{
    private readonly View _view;

    private Player CurrentPlayer { get; set; }
    private Player Opponent { get; set; }
    private List<Card> PlayableCards { get; set; }
    private int SelectedPlayIndex { get; set; }
    public bool GameOn { get; set; } = true;
    public bool TurnOn { get; set; } = true;

    public PlayerTurn(View view)
    {
        _view = view;
    }

    public bool PlayTurn(Player firstPlayer, Player secondPlayer)
    {
        StartPlayerTurn(firstPlayer);
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
        bool continueTurn = true;
        while (continueTurn)
        {
            continueTurn = HandleTurnActions(firstPlayer, secondPlayer);
        }
    }

    private bool HandleTurnActions(Player firstPlayer, Player secondPlayer)
    {
        _view.ShowGameInfo(firstPlayer.ToPlayerInfo(), secondPlayer.ToPlayerInfo());
        NextPlay turnActionsSelections = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();

        switch(turnActionsSelections)
        {
            case NextPlay.ShowCards:
                HandleShowCardsActions(firstPlayer, secondPlayer);
                break;
            case NextPlay.PlayCard:
                HandlePlayCardAction(firstPlayer, secondPlayer);
                if (!GameOn) return false;
                break;
            case NextPlay.EndTurn:
                HandleEndTurnAction(firstPlayer, secondPlayer);
                return false;
            case NextPlay.GiveUp:
                HandleGiveUpAction(secondPlayer);
                return false;
        }
        return true;
    }
    private void HandlePlayCardAction(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;

        PlayableCards = Play.GetPlayableCards(CurrentPlayer.Hand, CurrentPlayer.Fortitude);
        List<string> formattedPlayableCards = Play.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude);

        SelectedPlayIndex = _view.AskUserToSelectAPlay(formattedPlayableCards);

        if (SelectedPlayIndex == -1)
            return;

        ExecuteCardPlay();
    }

    private void ExecuteCardPlay()
    {
        AnnounceCardPlay();
        int cardDamage = CalculateCardDamage();
        AnnounceDamageToOpponent(cardDamage);

        bool hasLost = ApplyCardDamageToOpponent(cardDamage);
        if (hasLost)
        {
            EndGame(CurrentPlayer);
            return;
        }

        ApplyCardEffect();
    }

    private void AnnounceCardPlay()
    {
        string selectedPlay = Play.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude)[SelectedPlayIndex];
        _view.SayThatPlayerIsTryingToPlayThisCard(CurrentPlayer.Superstar.Name, selectedPlay);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private int CalculateCardDamage()
    {
        List<Play> playablePlays = Play.GetPlayablePlays(CurrentPlayer.Hand, CurrentPlayer.Fortitude);
        return playablePlays[SelectedPlayIndex].GetCardDamageAsInt();
    }

    private void AnnounceDamageToOpponent(int cardDamage)
    {
        _view.SayThatSuperstarWillTakeSomeDamage(Opponent.Superstar.Name, cardDamage);
    }

    private bool ApplyCardDamageToOpponent(int cardDamage)
    {
        return Opponent.ReceiveDamage(cardDamage);
    }

    private void ApplyCardEffect()
    {
        Card playedCard = PlayableCards[SelectedPlayIndex];
        int indexOfCardInHand = CurrentPlayer.Hand.FindIndex(card => ReferenceEquals(card, playedCard));
        CurrentPlayer.ApplyDamage(indexOfCardInHand);
    }

    private void HandleEndTurnAction(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;

        if (Opponent.HasEmptyArsenal())
        {
            EndGame(CurrentPlayer);
        }
        TurnOn = false;
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
                DisplayFormattedCards(firstPlayer, firstPlayer.Hand);
                break;

            case CardSet.RingArea:
                DisplayFormattedCards(firstPlayer, firstPlayer.RingArea);
                break;

            case CardSet.RingsidePile:
                DisplayFormattedCards(firstPlayer, firstPlayer.Ringside);
                break;

            case CardSet.OpponentsRingArea:
                DisplayFormattedCards(secondPlayer, secondPlayer.RingArea);
                break;

            case CardSet.OpponentsRingsidePile:
                DisplayFormattedCards(secondPlayer, secondPlayer.Ringside);
                break;
        }
    }
    private void DisplayFormattedCards(Player player, List<Card> cardSet)
    {
        List<string> formattedCards = player.GetFormattedCardsInfo(cardSet);
        _view.ShowCards(formattedCards);
    }
}