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
        Superstar playerSuperstar = player.Superstar;
        return AbilityNotUsed(playerSuperstar) &&
               AbilityActivationIsInMenu(playerSuperstar) &&
               SuperstarCanUseAbility(playerSuperstar, player);
    }

    private bool AbilityNotUsed(Superstar superstar)
    {
        return !superstar.HasUsedAbility;
    }

    private bool AbilityActivationIsInMenu(Superstar superstar)
    {
        return superstar.ActivationMoment == AbilityActivation.InMenu;
    }

    private bool SuperstarCanUseAbility(Superstar superstar, Player player)
    {
        return superstar.CanUseAbility(player);
    }

    private void ActivateStartOfTurnAbility(Player firstPlayer, Player secondPlayer)
    {
        Superstar firstPlayerSuperstar = firstPlayer.Superstar;
        firstPlayerSuperstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.StartOfTurn);
    }

    private void ActivateAutomaticSuperstarAbility(Player firstPlayer, Player secondPlayer)
    {
        Superstar firstPlayerSuperstar = firstPlayer.Superstar;
        if (AbilityNotUsed(firstPlayerSuperstar) && ActivationIsAutomatic(firstPlayerSuperstar))
        {
            firstPlayerSuperstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.Automatic);
        }
    }

    private bool ActivationIsAutomatic(Superstar superstar)
    {
        return superstar.ActivationMoment == AbilityActivation.Automatic;
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
        Superstar playerSuperstar = player.Superstar;
        playerSuperstar.MarkAbilityAsUnused();
    }

}