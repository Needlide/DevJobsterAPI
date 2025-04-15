namespace DevJobsterAPI.Common.Exceptions.DatabaseExceptions;

public class UniqueConstraintViolationException : ConstraintViolationException
{
    public UniqueConstraintViolationException(string message, string constraintName, string tableName)
        : base(message, constraintName, tableName)
    {
    }

    public UniqueConstraintViolationException(string message, Exception innerException,
        string constraintName, string tableName)
        : base(message, innerException, constraintName, tableName)
    {
    }
}