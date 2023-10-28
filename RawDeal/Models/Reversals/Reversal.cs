namespace RawDeal.Models.Reversals;

public class Reversal : Card
{
    public override Card Clone()
    {
        return new Reversal
        {
            Title = Title,
            Types = new List<string>(Types),
            Subtypes = new List<string>(Subtypes),
            Fortitude = Fortitude,
            Damage = Damage,
            StunValue = StunValue,
            CardEffect = CardEffect
        };
    }
    public virtual bool CardCanReverseFromDeck(Card playedCard, Card cardToReverse, int playerFortitude)
    {
        if (IsMatchingSubtype(playedCard, cardToReverse) && IsFortitudeSufficient(playerFortitude, cardToReverse))
        {
            return true;
        }
        return false;
    }

    private bool IsMatchingSubtype(Card playedCard, Card cardToReverse)
    {
        string playedCardSubtype = playedCard.Subtypes.FirstOrDefault(); // Asumiendo que hay un solo subtype
        string reversalSubtype = TransformReversalSubtype(cardToReverse.Subtypes.FirstOrDefault());

        return playedCardSubtype == reversalSubtype;
    }

    private string TransformReversalSubtype(string reversalSubtype)
    {
        switch (reversalSubtype)
        {
            case "ReversalStrike":
                return "Strike";
            case "ReversalGrapple":
                return "Grapple";
            case "ReversalSubmission":
                return "Submission";
            // ... Agrega más casos según sea necesario
            default:
                return reversalSubtype;
        }
    }

    private bool IsFortitudeSufficient(int playerFortitude, Card cardToReverse)
    {
        int reversalFortitude = int.Parse(cardToReverse.Fortitude.Replace("F", ""));
        return playerFortitude >= reversalFortitude;
    }

}