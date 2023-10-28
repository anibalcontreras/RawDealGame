using RawDeal.Controllers;
using RawDealView;
namespace RawDeal.Models.Superstars;

public class Kane : Superstar
{
    private readonly View _view;
    private readonly PlayerActionsController _playerActionsController;
    public Kane(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.StartOfTurn;
        _playerActionsController = new PlayerActionsController(view);
    }
    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility)
        {
            _view.SayThatPlayerIsGoingToUseHisAbility(player.Superstar.Name, player.Superstar.SuperstarAbility);
            _view.SayThatSuperstarWillTakeSomeDamage(opponent.Superstar.Name, 1);
            _playerActionsController.ReceiveDamage(opponent, 1);
            MarkAbilityAsUsed();
        }
    }
}