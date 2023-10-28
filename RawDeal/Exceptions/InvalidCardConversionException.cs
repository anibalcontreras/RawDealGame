namespace RawDeal.Exceptions;

public class InvalidCardConversionException : Exception
{
    public InvalidCardConversionException(string message) : base(message) { }
}