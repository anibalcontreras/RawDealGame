using RawDealView;
using RawDeal.Models;
using RawDealView.Options;
using RawDeal.Models.Superstars;
namespace RawDeal.Controllers;

public class SuperstarAbilityController
{
    private readonly View _view;
    public SuperstarAbilityController(View view)
    {
        _view = view;
    }
    private bool CanActivateAbility(Player player)
    {
        return !player.Superstar.HasUsedAbility &&
               player.Superstar.ActivationMoment == AbilityActivation.InMenu &&
               player.Superstar.CanUseAbility(player);
    }
    private void ActivateStartOfTurnAbility(Player firstPlayer, Player secondPlayer)
    {
        firstPlayer.Superstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.StartOfTurn);
    }
    private void ActivateAutomaticSuperstarAbility(Player firstPlayer, Player secondPlayer)
    {
        if (!firstPlayer.Superstar.HasUsedAbility &&
            firstPlayer.Superstar.ActivationMoment == AbilityActivation.Automatic)
            firstPlayer.Superstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.Automatic);
    }
    public void ActivateSuperstarsAbility(Player firstPlayer, Player secondPlayer)
    {
        ActivateAutomaticSuperstarAbility(firstPlayer, secondPlayer);
        ActivateStartOfTurnAbility(firstPlayer, secondPlayer);
    }
    public NextPlay DetermineIfSuperstarCanActivateHisAbility(Player player)
    {
        if (CanActivateAbility(player))
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();

        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }
    public void ResetAbilityUsage(Player player)
    {
        player.Superstar.MarkAbilityAsUnused();
    }
}