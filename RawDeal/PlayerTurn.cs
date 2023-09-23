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
        firstPlayer.DrawCard();
        _view.SayThatATurnBegins(firstPlayer.Superstar.Name);
        bool turnOn = true;
        while (turnOn)
            HandleTurnActions(firstPlayer, secondPlayer, ref turnOn, ref gameOn);
            return gameOn;
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

                List<Card> playableCards = Play.GetPlayableCards(firstPlayer.Hand, firstPlayer.Fortitude);
                List<string> formattedPlayableCards = Play.GetFormattedPlayableCards(playableCards, firstPlayer.Fortitude);

                int indexPlay = _view.AskUserToSelectAPlay(formattedPlayableCards);

                if (indexPlay == -1)
                    break;

                string selectedPlay = formattedPlayableCards[indexPlay];
                _view.SayThatPlayerIsTryingToPlayThisCard(firstPlayer.Superstar.Name, selectedPlay);
                _view.SayThatPlayerSuccessfullyPlayedACard();

                List<Play> playablePlays = Play.GetPlayablePlays(firstPlayer.Hand, firstPlayer.Fortitude);
                int cardDamage = playablePlays[indexPlay].GetCardDamageAsInt();
                _view.SayThatSuperstarWillTakeSomeDamage(secondPlayer.Superstar.Name, cardDamage);

                Card playedCard = playableCards[indexPlay];
                int indexOfCardInHand = firstPlayer.Hand.FindIndex(card => ReferenceEquals(card, playedCard));

                bool hasLost = secondPlayer.ReceiveDamage(cardDamage);
                if (hasLost)
                {
                    turnOn = false;
                    gameOn = false;
                    _view.CongratulateWinner(firstPlayer.Superstar.Name);
                    return;
                }
                firstPlayer.ApplyDamage(indexOfCardInHand);
                break;
            case NextPlay.EndTurn:
                if (secondPlayer.HasEmptyArsenal())
                {
                    gameOn = false;
                    _view.CongratulateWinner(firstPlayer.Superstar.Name);
                }
                turnOn = false;
                break;
            case NextPlay.GiveUp:
                turnOn = false;
                gameOn = false;
                _view.CongratulateWinner(firstPlayer.Superstar.Name);
                break;
        }
    }
    private void HandleShowCardsActions(Player firstPlayer, Player secondPlayer)
    {
        CardSet showCardsActionsSelection = _view.AskUserWhatSetOfCardsHeWantsToSee();
        
        switch(showCardsActionsSelection)
        {
            case CardSet.Hand:
                List<string> formattedHandCards = firstPlayer.GetFormattedCardsInfo(firstPlayer.Hand);
                _view.ShowCards(formattedHandCards);
                break;

            case CardSet.RingArea:
                List<string> formattedRingAreaCards = firstPlayer.GetFormattedCardsInfo(firstPlayer.RingArea);
                _view.ShowCards(formattedRingAreaCards);
                break;

            case CardSet.RingsidePile:
                List<string> formattedRingsideCards = firstPlayer.GetFormattedCardsInfo(firstPlayer.Ringside);
                _view.ShowCards(formattedRingsideCards);
                break;

            case CardSet.OpponentsRingArea:
                List<string> formattedOpponentsRingAreaCards = secondPlayer.GetFormattedCardsInfo(secondPlayer.RingArea);
                _view.ShowCards(formattedOpponentsRingAreaCards);
                break;

            case CardSet.OpponentsRingsidePile:
                List<string> formattedOpponentsRingsideCards = secondPlayer.GetFormattedCardsInfo(secondPlayer.Ringside);
                _view.ShowCards(formattedOpponentsRingsideCards);
                break;
        }
    }
}