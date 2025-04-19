using DevJobsterAPI.Common.Exceptions.DatabaseExceptions;
using Npgsql;

namespace DevJobsterAPI.Database;

public static class DatabaseExceptionHandler
{
    public static DatabaseException CatchDatabaseException(PostgresException exception)
    {
        var constraintName = exception.ConstraintName ?? string.Empty;
        var tableName = exception.TableName ?? string.Empty;

        throw exception.SqlState switch
        {
            "23505" => // unique_violation
                new UniqueConstraintViolationException($"A user with this {constraintName} already exists.", exception,
                    constraintName, tableName),
            "23503" => // foreign_key_violation
                new ForeignKeyViolationException("Referenced entity does not exist.", exception, constraintName,
                    tableName),
            "23514" => // check_violation
                new ConstraintViolationException($"The value violates a check constraint: {constraintName}", exception,
                    constraintName, tableName),
            "23502" => // not_null_violation
                new ConstraintViolationException($"The value violates not null constraint: {constraintName}", exception,
                    constraintName, tableName),
            "23P01" => // exclusion_constraint_violation
                new ConstraintViolationException($"The value violates exclusion constraint: {constraintName}",
                    exception, constraintName, tableName),
            _ => new DatabaseException("A database error occurred.", exception)
        };
    }
}