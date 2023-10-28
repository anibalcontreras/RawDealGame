using RawDealView;
using RawDeal.Models;
using RawDealView.Options;
namespace RawDeal.Controllers;
public class CardDisplayController
{
    private readonly View _view;
    public CardDisplayController(View view)
    {
        _view = view;
    }
    public void HandleShowCardsActions(Player firstPlayer, Player secondPlayer)
    {
        CardSet showCardsActionsSelection = _view.AskUserWhatSetOfCardsHeWantsToSee();

        switch (showCardsActionsSelection)
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