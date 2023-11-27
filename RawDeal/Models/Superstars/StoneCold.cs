using RawDeal.Controllers;
using RawDealView;
namespace RawDeal.Models.Superstars;

public class StoneCold : Superstar
{
    private readonly View _view;
    private readonly PlayerActionsController _playerActionsController;
    private readonly int _numberOfCardsToDraw = 1;

    public StoneCold(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.InMenu;
        _playerActionsController = new PlayerActionsController(view);
    }

    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (CanActivateAbility(player, activationTime))
        {
            UseAbility(player, opponent);
        }
    }

    private bool CanActivateAbility(Player player, AbilityActivation activationTime)
    {
        int arsenalCount = player.GetArsenal().Count;
        return activationTime == ActivationMoment && !HasUsedAbility && arsenalCount > 0;
    }

    public override bool CanUseAbility(Player player)
    {
        List<Card> arsenal = player.GetArsenal();
        return !HasUsedAbility && arsenal.Count > 0;
    }

    private void UseAbility(Player player, Player opponent)
    {
        AnnounceAbilityUsage(player);
        DrawCardFromArsenal(player);
        ReturnCardFromHandToArsenalBottom(player);
        MarkAbilityAsUsed();
    }

    private void AnnounceAbilityUsage(Player player)
    {
        string playerName = player.Superstar.Name;
        string superstarAbility = player.Superstar.SuperstarAbility;
        _view.SayThatPlayerIsGoingToUseHisAbility(playerName, superstarAbility);
    }

    private void DrawCardFromArsenal(Player player)
    {
        string playerName = player.Superstar.Name;
        _playerActionsController.DrawCard(player);
        _view.SayThatPlayerDrawCards(playerName, _numberOfCardsToDraw);
    }

    private void ReturnCardFromHandToArsenalBottom(Player player)
    {
        List<Card> hand = player.GetHand();
        List<string> formattedCardsInfo = player.GetFormattedCardsInfo(hand);
        string playerName = player.Superstar.Name;
        int selectedCardIndex = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(playerName, formattedCardsInfo);
        Card selectedCard = hand[selectedCardIndex];
        hand.Remove(selectedCard);
        List<Card> arsenal = player.GetArsenal();
        arsenal.Insert(0, selectedCard);
    }
}