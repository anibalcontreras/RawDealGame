using RawDealView;
namespace RawDeal.Models.Superstars;
public class Mankind : Superstar
{
    private readonly View _view;
    public Mankind(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.Automatic;
    }
    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility)
            UseAbility(player, opponent);
    }
    public override void UseAbility(Player player, Player opponent)
    {
        DrawCardsAtStartOfTurn(player);
        MarkAbilityAsUsed();
    }
     public void DrawCardsAtStartOfTurn(Player player)
     {
         if (player.GetArsenal().Count == 1)
         {
             player.DrawCard();
             player.DrawCard();
         } else
             player.DrawCard();
     }
}