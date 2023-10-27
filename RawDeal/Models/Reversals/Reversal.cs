namespace RawDeal.Models.Reversals;

public class Reversal : Card
{
    public virtual bool CanReverse(Card cardToReverse)
    {
        // Lógica específica de reversión
        return false;
    }
    public virtual void ApplyReversalEffect(Player player, Player opponent)
    {
        // Lógica para aplicar el efecto de reversión
    }
    
    public virtual bool CanReverseFromDeck(Card cardToReverse, int playerFortitude)
    {
        // Lógica específica de reversión
        int reversalFortitude = int.Parse(Fortitude);
        return false;
    }
}