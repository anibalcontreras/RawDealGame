namespace RawDeal.Exceptions;

public class DeserializationNullException : Exception
{
    private const string DefaultMessage = "Deserialization returned null or encountered an error.";

    public DeserializationNullException() : base(DefaultMessage) { }
}
