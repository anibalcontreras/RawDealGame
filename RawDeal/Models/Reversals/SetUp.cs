using RawDealView;
namespace RawDeal.Models.Reversals;

public class SetUp : Reversal
{
    public SetUp(View view) : base(view) { }
    
    public override bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        return false;
    }
    
    public override bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player)
    {
        return true;
    }
    
    protected override bool IsReversalInHand(Card cardInHand, Player player)
    {
        return false;
    }
    
    protected override bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard)
    {
        return false;
    }
    
    protected override bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        return false;
    }
}