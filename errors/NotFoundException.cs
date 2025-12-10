namespace luchito_net.Errors
{
    public class NotFoundException(string message, Exception? innerException = null) : Exception(message, innerException)
    {
    }
}