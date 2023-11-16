using RawDealView;
namespace RawDeal.Models.Reversals;

public class ReversalSpecial : Reversal
{
    public ReversalSpecial(View view) : base(view) { }

    public bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player)
    {
        // Llama al método base para mantener las comprobaciones estándar
        if (!base.CanReverseFromHand(playedCard, cardInHand, player))
        {
            return false;
        }

        // Comprueba la lógica específica de ReversalSpecial
        return CanReverseSpecialCondition(playedCard);
    }

    private bool CanReverseSpecialCondition(Card playedCard)
    {
        // Asumimos que 'Damage' es una propiedad del tipo string y necesitamos convertirla a int
        // Es mejor usar int.TryParse para evitar excepciones en caso de que 'Damage' no sea un número válido
        if (int.TryParse(playedCard.Damage, out int damage))
        {
            // Verifica si el daño es de 7 o menos
            return damage <= 7;
        }

        // Si 'Damage' no es un número, no se puede revertir con esta carta especial
        return false;
    }
}
