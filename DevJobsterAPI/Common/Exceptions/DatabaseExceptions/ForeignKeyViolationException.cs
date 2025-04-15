namespace DevJobsterAPI.Common.Exceptions.DatabaseExceptions;

public class ForeignKeyViolationException : ConstraintViolationException
{
    public ForeignKeyViolationException(string message, string constraintName, string tableName)
        : base(message, constraintName, tableName)
    {
    }

    public ForeignKeyViolationException(string message, Exception innerException,
        string constraintName, string tableName)
        : base(message, innerException, constraintName, tableName)
    {
    }
}