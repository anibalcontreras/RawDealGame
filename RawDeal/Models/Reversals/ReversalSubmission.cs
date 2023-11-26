using RawDealView;

namespace RawDeal.Models.Reversals;

public class ReversalSubmission : Reversal
{
    public ReversalSubmission(View view) : base(view) {}

    // Implementar la lógica para determinar si una Submission puede ser revertida desde el mazo
    public override bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        if (ReversalCanTargetPlayedCard(reversalCard, playedCard) &&
            PlayerHasSufficientFortitude(reversalCard, player))
        {
            _view.SayThatCardWasReversedByDeck(player.Superstar.Name);
            return true;
        }
        return false;
    }

    // Implementar la lógica para determinar si una Submission puede ser revertida desde la mano
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

    // Verificar si la carta jugada es una Submission y puede ser objetivo de la reversión
    protected override bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard)
    {
        try
        {
            // Registro inicial para saber que el método ha sido llamado.
            Console.WriteLine(
                $"Checking if reversal card '{reversalCard.Title}' can target played card '{playedCard.Title}'.");

            // Comprobación de si la carta jugada es una acción.
            bool isPlayedCardAction = playedCard.PlayedAs.Equals("Maneuver", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"Played card '{playedCard.Title}' PlayedAs 'Maneuver': {isPlayedCardAction}");

            // Comprobación de si la carta de reversión tiene el subtipo 'ReversalAction'.
            bool hasReversalActionSubtype = playedCard.Subtypes.Contains("Submission");
            Console.WriteLine(
                $"Reversal card '{playedCard.Title}' contains 'ReversalSubmission' subtype: {hasReversalActionSubtype}");

            // Resultado final basado en las comprobaciones anteriores.
            bool canReverse = isPlayedCardAction && hasReversalActionSubtype;
            Console.WriteLine(
                $"Can reversal card '{reversalCard.Title}' reverse played card '{playedCard.Title}': {canReverse}");

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
