namespace RawDeal.Exceptions;

public class UndefinedPlayedException : Exception
{
    private const string DefaultMessage = "The play is undefined.";

    public UndefinedPlayedException() : base(DefaultMessage) { }
}