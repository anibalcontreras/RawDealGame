namespace RawDeal.Exceptions;

public class AbilityNotDefinedException : Exception
{
    private const string DefaultMessage = "The play is undefined.";
    
    public AbilityNotDefinedException() : base(DefaultMessage) { }
}