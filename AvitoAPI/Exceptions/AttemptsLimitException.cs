namespace AvitoAPI.Exceptions;

public class AttemptsLimitException : Exception
{
    public AttemptsLimitException(string msg) : base(msg)
    {
        
    }
}