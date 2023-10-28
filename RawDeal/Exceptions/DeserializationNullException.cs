namespace RawDeal.Exceptions;

public class DeserializationNullException : Exception
{
    public DeserializationNullException(string message) : base(message) { }
}