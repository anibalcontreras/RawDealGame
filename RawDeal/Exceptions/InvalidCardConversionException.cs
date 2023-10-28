namespace RawDeal.Exceptions;

public class InvalidCardConversionException : Exception
{ 
    private const string DefaultMessage = "The card conversion is invalid.";

    public InvalidCardConversionException() : base(DefaultMessage) { }
}