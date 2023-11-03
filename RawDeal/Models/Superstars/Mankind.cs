using RawDeal.Controllers;
using RawDealView;
namespace RawDeal.Models.Superstars;
public class Mankind : Superstar
{
    private readonly View _view;
    private readonly PlayerActionsController _playerActionsController;
    public Mankind(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.Automatic;
        _playerActionsController = new PlayerActionsController(view);
    }
    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility)
            UseAbility(player);
    }
    private void UseAbility(Player player)
    {
        DrawCardsAtStartOfTurn(player);
        MarkAbilityAsUsed();
    }
     private void DrawCardsAtStartOfTurn(Player player)
     {
         if (player.GetArsenal().Count == 1)
         {
             _playerActionsController.DrawCard(player);
             _playerActionsController.DrawCard(player);
         } else
             _playerActionsController.DrawCard(player);
     }
     
     public override int CalculateDamage(int originalDamage)
     {
         return originalDamage - 1;
     }

}