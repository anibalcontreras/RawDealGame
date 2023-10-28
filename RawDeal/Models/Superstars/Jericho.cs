using RawDealView;
namespace RawDeal.Models.Superstars;

public class Jericho : Superstar
{
    private readonly View _view;
    
    public Jericho(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.InMenu;
    }
    
    public override bool CanUseAbility(Player player)
    {
        return !HasUsedAbility && player.GetHand().Count >= 1;
    }
    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility && CanUseAbility(player))
            UseAbility(player, opponent);
    }
    private void UseAbility(Player player, Player opponent)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.Superstar.Name, player.Superstar.SuperstarAbility);
        DiscardCard(player);
        OpponentDiscardCard(opponent);
        MarkAbilityAsUsed();
    }
    private void DiscardCard(Player player)
    {
        List<string> handCards = player.GetHand().Select(card => card.ToString()).ToList();
        int selectedCardIndex = _view.AskPlayerToSelectACardToDiscard(handCards,
            player.Superstar.Name,
            player.Superstar.Name,
            1);
        Card selectedCard = player.GetHand()[selectedCardIndex];
        player.GetHand().Remove(selectedCard);
        player.GetRingside().Add(selectedCard);
    }
    private void OpponentDiscardCard(Player opponent)
    {
        List<string> handCards = opponent.GetHand().Select(card => card.ToString()).ToList();
        int selectedCardIndex = _view.AskPlayerToSelectACardToDiscard(handCards, 
            opponent.Superstar.Name,
            opponent.Superstar.Name,
            1);
        Card selectedCard = opponent.GetHand()[selectedCardIndex];
        opponent.GetHand().Remove(selectedCard);
        opponent.GetRingside().Add(selectedCard);
    }
}