namespace RawDeal.Exceptions;

public class InvalidDeckException : Exception
{
    public InvalidDeckException(string message) : base(message) { }
}