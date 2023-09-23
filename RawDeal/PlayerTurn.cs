namespace RawDeal;

using RawDealView;
using RawDealView.Options;
using RawDeal.Models;

public class PlayerTurn
{
    private readonly View _view;

    public PlayerTurn(View view)
    {
        _view = view;
    }

    public bool PlayTurn(Player firstPlayer, Player secondPlayer, ref bool gameOn)
    {
        StartPlayerTurn(firstPlayer);
        ExecutePlayerActions(firstPlayer, secondPlayer, ref gameOn);
        return gameOn;
    }

    private void StartPlayerTurn(Player player)
    {
        player.DrawCard();
        _view.SayThatATurnBegins(player.Superstar.Name);
    }

    private void ExecutePlayerActions(Player firstPlayer, Player secondPlayer, ref bool gameOn)
    {
        bool turnOn = true;
        while (turnOn)
        {
            HandleTurnActions(firstPlayer, secondPlayer, ref turnOn, ref gameOn);
        }
    }

    private void HandleTurnActions(Player firstPlayer, Player secondPlayer, ref bool turnOn, ref bool gameOn)
    {
        _view.ShowGameInfo(firstPlayer.ToPlayerInfo(), secondPlayer.ToPlayerInfo());
        NextPlay turnActionsSelections = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();

        switch(turnActionsSelections)
        {
            case NextPlay.ShowCards:
                HandleShowCardsActions(firstPlayer, secondPlayer);
                break;
            case NextPlay.PlayCard:
                HandlePlayCardAction(firstPlayer, secondPlayer, ref turnOn, ref gameOn);
                break;
            case NextPlay.EndTurn:
                HandleEndTurnAction(firstPlayer, secondPlayer, ref turnOn, ref gameOn);
                break;
            case NextPlay.GiveUp:
                HandleGiveUpAction(firstPlayer, ref turnOn, ref gameOn);
                break;
        }
    }

    private void HandlePlayCardAction(Player firstPlayer, Player secondPlayer, ref bool turnOn, ref bool gameOn)
    {
        List<Card> playableCards = Play.GetPlayableCards(firstPlayer.Hand, firstPlayer.Fortitude);
        List<string> formattedPlayableCards = Play.GetFormattedPlayableCards(playableCards, firstPlayer.Fortitude);

        int indexPlay = _view.AskUserToSelectAPlay(formattedPlayableCards);

        if (indexPlay == -1)
            return;

        ExecuteCardPlay(firstPlayer, secondPlayer, playableCards, formattedPlayableCards, indexPlay, ref turnOn, ref gameOn);
    }

    private void ExecuteCardPlay(Player firstPlayer, Player secondPlayer, List<Card> playableCards, List<string> formattedPlayableCards, int indexPlay, ref bool turnOn, ref bool gameOn)
    {
        AnnounceCardPlay(firstPlayer, formattedPlayableCards, indexPlay);
        
        int cardDamage = CalculateCardDamage(firstPlayer, indexPlay);
        AnnounceDamageToOpponent(secondPlayer, cardDamage);

        bool hasLost = ApplyCardDamageToOpponent(secondPlayer, cardDamage);
        if (hasLost)
        {
            EndGame(firstPlayer, ref turnOn, ref gameOn);
            return;
        }

        ApplyCardEffect(firstPlayer, playableCards, indexPlay);
    }

    private void AnnounceCardPlay(Player player, List<string> formattedPlayableCards, int indexPlay)
    {
        string selectedPlay = formattedPlayableCards[indexPlay];
        _view.SayThatPlayerIsTryingToPlayThisCard(player.Superstar.Name, selectedPlay);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private int CalculateCardDamage(Player player, int indexPlay)
    {
        List<Play> playablePlays = Play.GetPlayablePlays(player.Hand, player.Fortitude);
        return playablePlays[indexPlay].GetCardDamageAsInt();
    }

    private void AnnounceDamageToOpponent(Player opponent, int cardDamage)
    {
        _view.SayThatSuperstarWillTakeSomeDamage(opponent.Superstar.Name, cardDamage);
    }

    private bool ApplyCardDamageToOpponent(Player opponent, int cardDamage)
    {
        return opponent.ReceiveDamage(cardDamage);
    }

    private void ApplyCardEffect(Player player, List<Card> playableCards, int indexPlay)
    {
        Card playedCard = playableCards[indexPlay];
        int indexOfCardInHand = player.Hand.FindIndex(card => ReferenceEquals(card, playedCard));
        player.ApplyDamage(indexOfCardInHand);
    }


    private void HandleEndTurnAction(Player firstPlayer, Player secondPlayer, ref bool turnOn, ref bool gameOn)
    {
        if (secondPlayer.HasEmptyArsenal())
        {
            EndGame(firstPlayer, ref turnOn, ref gameOn);
        }
        turnOn = false;
    }

    private void HandleGiveUpAction(Player firstPlayer, ref bool turnOn, ref bool gameOn)
    {
        EndGame(firstPlayer, ref turnOn, ref gameOn);
    }

    private void EndGame(Player winningPlayer, ref bool turnOn, ref bool gameOn)
    {
        turnOn = false;
        gameOn = false;
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