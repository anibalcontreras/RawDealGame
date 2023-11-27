namespace RawDeal.Exceptions;

public class CardReversedException : Exception
{
    public CardReversedException() : base("La carta se revirtió.") { }
}