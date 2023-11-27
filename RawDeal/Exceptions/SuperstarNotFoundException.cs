namespace RawDeal.Exceptions;

public class SuperstarNotFoundException : Exception
{
    private const string DefaultMessage = "The superstar was not found.";
    
    public SuperstarNotFoundException() : base(DefaultMessage) { }
}