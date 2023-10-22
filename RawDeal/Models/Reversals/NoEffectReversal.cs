using RawDealView;
namespace RawDeal.Models.Reversals;

public class NoEffectReversal : Reversal
{
    public NoEffectReversal(View view) : base(view) { }

    public override bool Apply(Player player, Player opponent, Card card)
    {
        Card reversalCard = card;
        Console.WriteLine("La reversal card es " + reversalCard.Title);
        if (reversalCard.Types.Contains("Reversal"))
        {
            if (player.Fortitude >= int.Parse(reversalCard.Fortitude))
            {
                _view.SayThatCardWasReversedByDeck(player.Superstar.Name);
                return true;
            }
        }
        return false;
    }
}