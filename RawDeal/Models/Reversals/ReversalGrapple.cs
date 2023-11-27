using RawDeal.Exceptions;
using RawDealView;

namespace RawDeal.Models.Reversals;

public class ReversalGrapple : Reversal
{
    public ReversalGrapple(View view) : base(view) {}
    protected override bool ReversalCanTargetPlayedCard(Card playedCard)
    {
        try
        {
            bool isPlayedCardAction = playedCard.PlayedAs.Equals("Maneuver", StringComparison.OrdinalIgnoreCase);
            bool hasReversalActionSubtype = playedCard.Subtypes.Contains("Grapple");
            bool canReverse = isPlayedCardAction && hasReversalActionSubtype;
            return canReverse;
        }
        catch (CardPropertyNotFoundException)
        {
            return false;
        }
    }
}
