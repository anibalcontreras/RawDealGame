namespace RawDeal.Models.Reversals;

public class NoEffectReversal : Reversal
{
    public NoEffectReversal()
    {
        CardEffect = "No effect";
    }
    public override bool CanReverse(Card cardToReverse)
    {
        return true;
    }
    public override void ApplyReversalEffect(Player player, Player opponent)
    {
        // No effect
    }
}