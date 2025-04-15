using DevJobsterAPI.Common.Exceptions.DatabaseExceptions;
using Npgsql;

namespace DevJobsterAPI.Database;

public static class DatabaseExceptionHandler
{
    public static DatabaseException CatchDatabaseException(PostgresException exception)
    {
        var constraintName = exception.ConstraintName ?? string.Empty;
        var tableName = exception.TableName ?? string.Empty;

        switch (exception.SqlState)
        {
            case "23505": // unique_violation
                throw new UniqueConstraintViolationException(
                    $"A user with this {constraintName} already exists.",
                    exception, constraintName, tableName);

            case "23503": // foreign_key_violation
                throw new ForeignKeyViolationException(
                    "Referenced entity does not exist.",
                    exception, constraintName, tableName);

            case "23514": // check_violation
                throw new ConstraintViolationException(
                    $"The value violates a check constraint: {constraintName}",
                    exception, constraintName, tableName);

            case "23502": // not_null_violation
                throw new ConstraintViolationException(
                    $"The value violates not null constraint: {constraintName}",
                    exception, constraintName, tableName);

            case "23P01": // exclusion_constraint_violation
                throw new ConstraintViolationException(
                    $"The value violates exclusion constraint: {constraintName}",
                    exception, constraintName, tableName);

            default:
                throw new DatabaseException("A database error occurred.", exception);
        }
    }
}