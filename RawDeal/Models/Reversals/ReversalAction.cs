using RawDeal.Exceptions;
using RawDealView;

namespace RawDeal.Models.Reversals;

public class ReversalAction : Reversal
{
    public ReversalAction(View view) : base(view) {}
    protected override bool ReversalCanTargetPlayedCard(Card playedCard)
    {
        try
        {
            bool isPlayedCardAction = playedCard.PlayedAs.Equals("Action", StringComparison.OrdinalIgnoreCase);
            bool canReverse = isPlayedCardAction;
            return canReverse;
        }
        catch (CardPropertyNotFoundException)
        {
            return false;
        }
    }
}
