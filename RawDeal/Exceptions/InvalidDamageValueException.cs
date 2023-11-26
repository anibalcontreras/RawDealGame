namespace RawDeal.Exceptions;

public class InvalidDamageValueException : Exception
{
    private const string DefaultMessage = "El valor de daño es inválido.";
    
    public InvalidDamageValueException() : base(DefaultMessage) { }
}