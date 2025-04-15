namespace DevJobsterAPI.Common.Exceptions.DatabaseExceptions;

public class DatabaseException : Exception
{
    protected DatabaseException(string message) : base(message)
    {
    }

    public DatabaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}