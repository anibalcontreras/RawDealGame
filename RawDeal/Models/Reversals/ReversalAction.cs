using RawDealView;

namespace RawDeal.Models.Reversals;

public class ReversalAction : Reversal
{
    public ReversalAction(View view) : base(view) {}

    // Implementar la lógica para determinar si una Acción puede ser revertida desde el mazo
    public override bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        if (ReversalCanTargetPlayedCard(reversalCard, playedCard) &&
            PlayerHasSufficientFortitude(reversalCard, player))
        {
            _view.SayThatCardWasReversedByDeck(player.Superstar.Name);
            // Lógica adicional para manejar que la carta va al montón Ringside, etc.
            return true;
        }
        return false;
    }

    // Implementar la lógica para determinar si una Acción puede ser revertida desde la mano
    public override bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player)
    {
        return IsReversalInHand(cardInHand, player) &&
               ReversalCanTargetPlayedCard(cardInHand, playedCard) &&
               PlayerHasSufficientFortitude(cardInHand, player);
    }

    // Determinar si la carta de reversión está en la mano del jugador
    protected override bool IsReversalInHand(Card cardInHand, Player player)
    {
        return player.GetHand().Contains(cardInHand);
    }

    // Verificar si la carta jugada es una Acción y puede ser objetivo de la reversión
    protected override bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard)
    {
        try
        {
            
            // Comprobación de si la carta jugada es una acción.
            bool isPlayedCardAction = playedCard.PlayedAs.Equals("Action", StringComparison.OrdinalIgnoreCase);

            // // Comprobación de si la carta de reversión tiene el subtipo 'ReversalAction'.
            // bool hasReversalActionSubtype = playedCard.Subtypes.Contains("");
            // Console.WriteLine($"Reversal card '{reversalCard.Title}' contains 'ReversalAction' subtype: {hasReversalActionSubtype}");

            // Resultado final basado en las comprobaciones anteriores.
            bool canReverse = isPlayedCardAction;
            return canReverse;
        }
        catch (NullReferenceException)
        {
            return false;
        }
        
    }

    // Comprobar si el jugador tiene suficiente Fortaleza para usar la reversión
    protected override bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        // Asumiendo que 'Fortitude' es una propiedad de tipo string en la clase Card y Player
        if (int.TryParse(reversalCard.Fortitude, out int reversalFortitude))
        {
            return player.Fortitude >= reversalFortitude;
        }
        return false;
    }
}
