using RawDealView;
namespace RawDeal.Models.Reversals;

public class NoEffectReversal : Reversal
{
    public NoEffectReversal(View view, Card card) : base(view, card) { }
    
    public override bool CanReverseFromDeck(Card playedCard)
    {
            return _card.Subtypes.Any(subType => playedCard.Subtypes.Contains(GetReversalTypeFromSubtype(subType)));
    }

    public override bool CanReverseFromHand(Card playedCard, Player player)
    {
        int playerFortitudeRating = player.Fortitude;
        if (playerFortitudeRating < int.Parse(_card.Fortitude))
            return false;
        
        return _card.Subtypes.Any(subType => playedCard.Types.Any(type => subType.Contains(GetReversalTypeFromSubtype(type))));
    }
    
    private string GetReversalTypeFromSubtype(string subtype)
    {
        switch (subtype)
        {
            case "ReversalGrapple":
                return "Grapple";
            case "ReversalStrike":
                return "Strike";
            case "ReversalSubmission":
                return "Submission";
            case "ReversalAction":
                return "Action";
            default:
                return "";
        }
    }
}