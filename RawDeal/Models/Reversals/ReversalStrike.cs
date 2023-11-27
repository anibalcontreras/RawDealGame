using RawDeal.Exceptions;
using RawDealView;

namespace RawDeal.Models.Reversals;

public class ReversalStrike : Reversal
{
    public ReversalStrike(View view) : base(view) {}
    protected override bool ReversalCanTargetPlayedCard(Card playedCard)
    {
        try
        {
            bool isPlayedCardAction = playedCard.PlayedAs.Equals("Maneuver", StringComparison.OrdinalIgnoreCase);
            bool hasReversalActionSubtype = playedCard.Subtypes.Contains("Strike");
            bool canReverse = isPlayedCardAction && hasReversalActionSubtype;
            return canReverse;
        }
        catch (CardPropertyNotFoundException)
        {
            return false;
        }
    }
}
