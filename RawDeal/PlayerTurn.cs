namespace RawDeal;

using RawDealView;
using RawDealView.Options;

public class PlayerTurn
{
    private readonly View _view;

    public PlayerTurn(View view)
    {
        _view = view;
    }

    public void PlayTurn(Player firstPlayer, Player secondPlayer)
    {

        firstPlayer.DrawCard();

        _view.SayThatATurnBegins(firstPlayer.Superstar.Name);

        bool turnOn = true;
        while (turnOn)
            HandleTurnActions(firstPlayer, secondPlayer, ref turnOn);
    }

    private void HandleTurnActions(Player firstPlayer, Player secondPlayer, ref bool turnOn)
    {
        _view.ShowGameInfo(firstPlayer.ToPlayerInfo(), secondPlayer.ToPlayerInfo());
        NextPlay turnActionsSelections = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();

        switch(turnActionsSelections)
        {
            case NextPlay.ShowCards:
                HandleShowCardsActions(firstPlayer, secondPlayer);
                break;
            case NextPlay.EndTurn:
                turnOn = false;
                break;
            case NextPlay.GiveUp:
                turnOn = false;
                break;
        }
    }

    private void HandleShowCardsActions(Player firstPlayer, Player secondPlayer)
    {
        CardSet showCardsActionsSelection = _view.AskUserWhatSetOfCardsHeWantsToSee();
        
        // Ver como limpiar este codigo para no repetir tanto List<string>
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