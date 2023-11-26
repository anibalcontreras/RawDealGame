using RawDealView;
namespace RawDeal.Models.Reversals;

public abstract class Reversal
{
    protected readonly View _view;
    protected Reversal(View view)
    {
        _view = view;
    }
    
    public abstract bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard);
    public abstract bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player);
    protected abstract bool IsReversalInHand(Card cardInHand, Player player);
    protected abstract bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard);
    protected abstract bool PlayerHasSufficientFortitude(Card reversalCard, Player player);
}