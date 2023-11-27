using RawDeal.Controllers;
using RawDealView;

namespace RawDeal.Models.Effects;
public class DrawCardEffect: Effect
{
    private readonly PlayerActionsController _playerActionsController;
    public DrawCardEffect(View view) : base(view)
    {
        _playerActionsController = new PlayerActionsController(view);
    }
    private const int NumberOfCardsToDraw = 1;
    public override bool Apply(Player player, Player opponent, Card card)
    {
        _playerActionsController.DiscardCard(player, card);
        _playerActionsController.DrawCard(player);
        _view.SayThatPlayerDrawCards(player.Superstar.Name, NumberOfCardsToDraw);
        return false;
    }
}