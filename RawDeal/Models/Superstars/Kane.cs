using RawDealView;
namespace RawDeal.Models.Superstars;

public class Kane : Superstar
{
    private readonly View _view;
    public Kane(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.StartOfTurn;
    }
    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility)
        {
            _view.SayThatPlayerIsGoingToUseHisAbility(player.Superstar.Name, player.Superstar.SuperstarAbility);
            _view.SayThatSuperstarWillTakeSomeDamage(opponent.Superstar.Name, 1);
            opponent.ReceiveDamage(1);
            MarkAbilityAsUsed();
        }
    }
}