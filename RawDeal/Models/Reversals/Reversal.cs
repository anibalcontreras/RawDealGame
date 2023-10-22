using RawDealView;
namespace RawDeal.Models.Reversals;

public abstract class Reversal
{
    protected readonly View _view;
    public Reversal(View view)
    {
        _view = view;
    }
    public abstract bool Apply(Player player, Player opponent, Card card);
}