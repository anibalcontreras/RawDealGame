using RawDealView;
namespace RawDeal.Models.Reversals;

public class ReversalSpecial : Reversal
{
    public ReversalSpecial(View view) : base(view) {}

    public override bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        if (CanReverseSpecialCondition(playedCard) && PlayerHasSufficientFortitude(reversalCard, player))
        {
            _view.SayThatCardWasReversedByDeck(player.Superstar.Name);
            return true;
        }
        return false;
    }

    public override bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player)
    {
        return IsReversalInHand(cardInHand, player) &&
               CanReverseSpecialCondition(playedCard) &&
               PlayerHasSufficientFortitude(cardInHand, player)
               && IsTheCardPlayedAsManeuver(playedCard);
    }

    protected override bool IsReversalInHand(Card cardInHand, Player player)
    {
        // Implementa la comprobación de si la carta está en la mano del jugador
        return player.GetHand().Contains(cardInHand);
    }

    protected override bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard)
    {
        // Esta implementación específica se ha movido al método CanReverseSpecialCondition
        return true; // Esta línea es un marcador de posición, la lógica está en CanReverseSpecialCondition
    }

    protected override bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        // Implementa la comprobación de la fortaleza del jugador
        return player.Fortitude >= int.Parse(reversalCard.Fortitude);
    }

    private bool CanReverseSpecialCondition(Card playedCard)
    {
        // Asumiendo que 'Damage' es una propiedad del tipo string y necesitamos convertirla a int
        if (int.TryParse(playedCard.Damage, out int damage))
        {
            // Verifica si el daño es de 7 o menos
            return damage <= 7;
        }
        
        return false;
    }

    private bool IsTheCardPlayedAsManeuver(Card playedCard)
    {
        if (playedCard.PlayedAs == "MANEUVER")
            return true;

        return false;
    }
}
