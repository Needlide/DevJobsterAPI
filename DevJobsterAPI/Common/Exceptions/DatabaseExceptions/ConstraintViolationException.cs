namespace DevJobsterAPI.Common.Exceptions.DatabaseExceptions;

public class ConstraintViolationException : DatabaseException
{
    protected ConstraintViolationException(string message, string constraintName, string tableName)
        : base(message)
    {
        ConstraintName = constraintName;
        TableName = tableName;
    }

    public ConstraintViolationException(string message, Exception innerException,
        string constraintName, string tableName)
        : base(message, innerException)
    {
        ConstraintName = constraintName;
        TableName = tableName;
    }

    public string ConstraintName { get; }
    public string TableName { get; }
}