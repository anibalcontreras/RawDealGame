namespace RawDeal.Exceptions;

public class EmptyFileException : Exception
{
 private const string DefaultMessage = "The file is empty.";
 
 public EmptyFileException() : base(DefaultMessage) { }
}