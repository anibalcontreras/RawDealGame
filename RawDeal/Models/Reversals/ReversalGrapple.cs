using RawDealView;

namespace RawDeal.Models.Reversals;

public class ReversalGrapple : Reversal
{
    public ReversalGrapple(View view) : base(view) {}
    
    public override bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        if (ReversalCanTargetPlayedCard(reversalCard, playedCard) &&
            PlayerHasSufficientFortitude(reversalCard, player))
        {
            _view.SayThatCardWasReversedByDeck(player.Superstar.Name);
            return true;
        }
        return false;
    }
    
    public override bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player)
    {
        return IsReversalInHand(cardInHand, player) &&
               ReversalCanTargetPlayedCard(cardInHand, playedCard) &&
               PlayerHasSufficientFortitude(cardInHand, player);
    }
    protected override bool IsReversalInHand(Card cardInHand, Player player)
    {
        return player.GetHand().Contains(cardInHand);
    }
    protected override bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard)
    {
        try
        {
            bool isPlayedCardAction = playedCard.PlayedAs.Equals("Maneuver", StringComparison.OrdinalIgnoreCase);
            bool hasReversalActionSubtype = playedCard.Subtypes.Contains("Grapple");
            bool canReverse = isPlayedCardAction && hasReversalActionSubtype;
            return canReverse;
        }
        catch (NullReferenceException)
        {
            return false;
        }
    }
    
    protected override bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        if (int.TryParse(reversalCard.Fortitude, out int reversalFortitude))
        {
            return player.Fortitude >= reversalFortitude;
        }
        return false;
    }
}
