namespace RawDeal.Exceptions;

public class ReversalNotFoundException : Exception
{
    private const string DefaultMessage = "The reversal was not found.";

    public ReversalNotFoundException() : base(DefaultMessage) { }
}
