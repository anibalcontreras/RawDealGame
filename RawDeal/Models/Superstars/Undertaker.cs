using RawDealView;
namespace RawDeal.Models.Superstars;

public class Undertaker : Superstar
{
    private readonly View _view;

    public Undertaker(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.InMenu;
    }

    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility && CanUseAbility(player))
            UseAbility(player, opponent);
    }

    private void UseAbility(Player player, Player opponent)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.Superstar.Name, player.Superstar.SuperstarAbility);
        DiscardTwoCards(player);
        RecoverCardFromRingsideToHand(player);
        MarkAbilityAsUsed();
    }
    
    public override bool CanUseAbility(Player player)
    {
        return !HasUsedAbility && player.GetHand().Count >= 2;
    }

    private void DiscardTwoCards(Player player)
    {
        for (int i = 0; i < 2; i++)
        {
            List<string> handCards = player.GetHand().Select(card => card.ToString()).ToList();
            int selectedCardIndex = _view.AskPlayerToSelectACardToDiscard(handCards, player.Superstar.Name, player.Superstar.Name, 2 - i);
            Card selectedCard = player.GetHand()[selectedCardIndex];
            player.GetHand().Remove(selectedCard);
            player.GetRingside().Add(selectedCard);
        }
    }

    private void RecoverCardFromRingsideToHand(Player player)
    {
        List<string> ringsideCards = player.GetRingside().Select(card => card.ToString()).ToList();
        int selectedCardIndex = _view.AskPlayerToSelectCardsToPutInHisHand(player.Superstar.Name, 1, ringsideCards);
        Card selectedCard = player.GetRingside()[selectedCardIndex];
        player.GetRingside().Remove(selectedCard);
        player.GetHand().Add(selectedCard);
    }
}