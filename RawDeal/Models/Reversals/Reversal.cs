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
        if (ReversalCanTargetPlayedCard(reversalCard, playedCard) &&
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
               ReversalCanTargetPlayedCard(cardInHand, playedCard) &&
               PlayerHasSufficientFortitude(cardInHand, player);
    }
    
    private bool IsReversalInHand(Card cardInHand, Player player)
    {
        return player.GetHand().Contains(cardInHand);
    }

    private bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard)
    {
        if (!reversalCard.Types.Contains("Reversal"))
        {
            return false;
        }
        try
        {
            // Verifica si la carta fue jugada como una maniobra y si el tipo coincide con el subtipo de reversión.
            if (playedCard.PlayedAs.Equals("Maneuver", StringComparison.OrdinalIgnoreCase) &&
                reversalCard.Subtypes.Any(subtype => subtype.Equals("Reversal" + playedCard.Subtypes.FirstOrDefault())))
            {
                return true;
            }
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Null reference exception");
            return false;
            // throw new UndefinedPlayedException();
        }
        
        // Si la carta fue jugada como una acción y el subtipo de reversión es 'ReversalAction'.
        if (playedCard.PlayedAs.Equals("Action", StringComparison.OrdinalIgnoreCase) &&
            reversalCard.Subtypes.Contains("ReversalAction"))
            return true;
        
        return false;
    }
    
    private bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        return player.Fortitude >= int.Parse(reversalCard.Fortitude);
    }
}