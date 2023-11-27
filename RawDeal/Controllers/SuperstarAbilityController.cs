using RawDealView;
using RawDeal.Models;
using RawDealView.Options;
using RawDeal.Models.Superstars;
using RawDeal.Utilities;

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
        return AbilityActivationIsInMenu(playerSuperstar) &&
               SuperstarCanUseAbility(playerSuperstar, player) &&
               AbilityNotUsed(playerSuperstar);
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
        if (!HasValidSuperstar(player)) return;

        ResetPlayerSuperstarAbility(player);
    }
    
    private bool HasValidSuperstar(Player player)
    {
        return player.HasValidSuperstar();
    }

    private void ResetPlayerSuperstarAbility(Player player)
    {
        player.Superstar.MarkAbilityAsUnused();
    }
    
    public void ActivateAbilityInMenu(Player firstPlayer, Player secondPlayer)
    {
        firstPlayer.Superstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.InMenu);
    }
}