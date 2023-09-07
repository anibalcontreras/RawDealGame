namespace RawDeal;

using RawDeal.Models;
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
            FirstAskUser(firstPlayer, secondPlayer, ref turnOn);
    }

    public void FirstAskUser(Player firstPlayer, Player secondPlayer, ref bool turnOn)
    {
        _view.ShowGameInfo(firstPlayer.ToPlayerInfo(), secondPlayer.ToPlayerInfo());
        NextPlay userSelection = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();

        switch(userSelection)
        {
            case NextPlay.ShowCards:
                AskUserWhatOptionWantToDo(firstPlayer, secondPlayer);
                break;
            case NextPlay.GiveUp:
                turnOn = false;
                break;
        }
    }
    public void AskUserWhatOptionWantToDo(Player firstPlayer, Player secondPlayer)
    {
        CardSet userSelection = _view.AskUserWhatSetOfCardsHeWantsToSee();
        
        switch(userSelection)
        {
            case CardSet.Hand:
                // Mostrar cartas en la mano.
                List<string> formattedHandCards = firstPlayer.GetFormattedCardsInfo(firstPlayer.Hand);
                _view.ShowCards(formattedHandCards);
                break;

            case CardSet.RingArea:
                Console.WriteLine("Mostrar cartas en el área de anillo.");
                // Mostrar cartas en el área de anillo del jugador.
                List<string> formattedRingAreaCards = firstPlayer.GetFormattedCardsInfo(firstPlayer.RingArea);
                _view.ShowCards(formattedRingAreaCards);
                break;

            case CardSet.RingsidePile:
                // Mostrar cartas en la pila de Ringside del jugador.
                List<string> formattedRingsideCards = firstPlayer.GetFormattedCardsInfo(firstPlayer.Ringside);
                _view.ShowCards(formattedRingsideCards);
                break;

            case CardSet.OpponentsRingArea:
                // Mostrar cartas en el área de anillo del oponente.
                List<string> formattedOpponentsRingAreaCards = secondPlayer.GetFormattedCardsInfo(secondPlayer.RingArea);
                _view.ShowCards(formattedOpponentsRingAreaCards);
                break;

            case CardSet.OpponentsRingsidePile:
                // Mostrar cartas en la pila de Ringside del oponente.
                List<string> formattedOpponentsRingsideCards = secondPlayer.GetFormattedCardsInfo(secondPlayer.Ringside);
                _view.ShowCards(formattedOpponentsRingsideCards);
                break;
        }
    }
}