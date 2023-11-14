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
            string playerName = player.Superstar.Name;
            string opponentName = opponent.Superstar.Name;
            string superstarAbility = player.Superstar.SuperstarAbility;

            _view.SayThatPlayerIsGoingToUseHisAbility(playerName, superstarAbility);
            _view.SayThatSuperstarWillTakeSomeDamage(opponentName, 1);
            _playerActionsController.ReceiveDamage(opponent, 1, null, player);
            MarkAbilityAsUsed();
        }
    }
}