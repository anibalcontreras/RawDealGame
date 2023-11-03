using RawDealView;
namespace RawDeal.Models.Effects;
public abstract class Effect
{
    protected readonly View _view;
    protected Effect(View view)
    {
        _view = view;
    }
    public abstract bool Apply(Player player, Player opponent, Card card);
}