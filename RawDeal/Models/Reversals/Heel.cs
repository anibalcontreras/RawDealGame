using RawDealView;
using RawDeal.Controllers;
namespace RawDeal.Models.Reversals;

public class Heel : Reversal
{
    public Heel(View view) : base(view) { }

    private readonly PlayerActionsController _playerActionsController;
    public override bool CanReverseFromDeck(Card reversalCard, Player player, Card playedCard)
    {
        // La lógica para revertir desde el mazo es específica y no aplica aquí.
        return false;
    }

    public override bool CanReverseFromHand(Card playedCard, Card cardInHand, Player player)
    {
        // Esta reversión puede revertir cualquier maniobra si se juega desde la mano.
        return IsReversalInHand(cardInHand, player) &&
               PlayerHasSufficientFortitude(cardInHand, player);
    }

    protected override bool IsReversalInHand(Card cardInHand, Player player)
    {
        return player.GetHand().Contains(cardInHand);
    }

    protected override bool ReversalCanTargetPlayedCard(Card reversalCard, Card playedCard)
    {
        // No hay restricciones específicas de tipo o daño para esta reversión.
        return true;
    }

    protected override bool PlayerHasSufficientFortitude(Card reversalCard, Player player)
    {
        return player.Fortitude >= int.Parse(reversalCard.Fortitude);
    }

    // Método adicional para manejar la acción de robar una carta.
    public void DrawCardIfPlayedFromHand(Player player)
    {
        _view.SayThatPlayerDrawCards(player.Superstar.Name, 1);
        _playerActionsController.DrawCard(player);
    }
}