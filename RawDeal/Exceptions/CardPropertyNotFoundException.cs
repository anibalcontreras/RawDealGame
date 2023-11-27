namespace RawDeal.Exceptions;

public class CardPropertyNotFoundException : Exception
{
    public CardPropertyNotFoundException(string message) : base(message) { }
    
    public CardPropertyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}