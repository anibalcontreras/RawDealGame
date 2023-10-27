using RawDealView;
namespace RawDeal.Models.Effects;
public class DrawCardEffect: Effect
{
    public DrawCardEffect(View view) : base(view)
    {
    }
    public override bool Apply(Player player, Player opponent, Card card)
    {
        player.DiscardCard(card); ;
        player.DrawCard();
        _view.SayThatPlayerDrawCards(player.Superstar.Name, 1);
        return false;
    }
}