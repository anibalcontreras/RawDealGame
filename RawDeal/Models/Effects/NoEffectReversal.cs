using RawDealView;
namespace RawDeal.Models.Effects;

public class NoEffectReversal : Effect
{

    public NoEffectReversal(View view) : base(view)
    {
    }

    public override bool Apply(Player player, Player opponent, Card card)
    {
        return false;
    }
}