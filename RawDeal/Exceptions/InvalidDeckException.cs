namespace RawDeal.Exceptions;

public class InvalidDeckException : Exception
{
    private const string DefaultMessage = "The deck is invalid.";

    public InvalidDeckException() : base(DefaultMessage) { }
}