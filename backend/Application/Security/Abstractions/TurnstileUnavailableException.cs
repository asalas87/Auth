namespace Application.Security.Abstractions;

public class TurnstileUnavailableException : Exception
{
    public TurnstileUnavailableException(string message, Exception innerException) 
        : base(message, innerException) { }
}
