using RawDealView;
namespace RawDeal.Models.Reversals;

public abstract class Reversal
{
    private readonly View _view;
    protected Reversal(View view)
    {
        _view = view;
    }
    
    public bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        if (ReversalCanTargetPlayedCard(playedCard) &&
            PlayerHasSufficientFortitude(reversalCard, player))
        {
            _view.SayThatCardWasReversedByDeck(player.Superstar.Name);
            return true;
        }
        return false;
    }
    
    public bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player)
    {
        return IsReversalInHand(cardInHand, player) &&
               ReversalCanTargetPlayedCard(playedCard) &&
               PlayerHasSufficientFortitude(cardInHand, player);
    }
    
    private bool IsReversalInHand(Card cardInHand, Player player)
    {
        return player.GetHand().Contains(cardInHand);
    }
    
    private bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        if (int.TryParse(reversalCard.Fortitude, out int reversalFortitude))
            return player.Fortitude >= reversalFortitude;
        return false;
    }
    
    protected abstract bool ReversalCanTargetPlayedCard(Card playedCard);
}