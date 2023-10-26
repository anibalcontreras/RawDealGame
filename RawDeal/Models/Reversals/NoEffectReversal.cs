using RawDealView;
namespace RawDeal.Models.Reversals;

public class NoEffectReversal : Reversal
{
    public NoEffectReversal(View view, Card reversalCard) : base(view, reversalCard) { }

    public override bool Apply(Player player, Player opponent, Card playedCard)
    {
        // Verificar si la carta jugada coincide con los subtipos de la carta de reversión y si el jugador tiene suficiente fortaleza
        if (Subtypes.Any(subtype => playedCard.Subtypes.Contains(GetReversalTypeFromSubtype(subtype))) && player.Fortitude >= int.Parse(Fortitude))
        {
            _view.SayThatCardWasReversedByDeck(player.Superstar.Name);
            return true;
        }
        return false;
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