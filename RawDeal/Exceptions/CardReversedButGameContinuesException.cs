namespace RawDeal.Exceptions;

public class CardReversedButGameContinuesException : Exception
{
    public CardReversedButGameContinuesException() : base("La carta se revirtió, pero el juego continúa.") { }
}