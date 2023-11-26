using RawDealView;
namespace RawDeal.Models.Reversals;

public class ReversalStrikeSpecial : Reversal
{
    private const int DamageThreshold = 7;

    public ReversalStrikeSpecial(View view) : base(view) {}

    public override bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        // La lógica para revertir desde el mazo es específica y no aplica aquí.
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
        // Esta comprobación se hace específica para maniobras de tipo Strike
        return playedCard.Subtypes.Contains("Strike") && IsDamageWithinThreshold(playedCard);
    }

    protected override bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        return player.Fortitude >= int.Parse(reversalCard.Fortitude);
    }

    private bool IsDamageWithinThreshold(Card playedCard)
    {
        // Verifica si el daño de la maniobra de Strike es 7 o menos.
        return int.TryParse(playedCard.Damage, out int damage) && damage <= DamageThreshold;
    }
}