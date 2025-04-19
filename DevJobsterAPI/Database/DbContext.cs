using System.Data;
using DevJobsterAPI.Database.Abstract;
using Npgsql;

namespace DevJobsterAPI.Database;

public class DbContext : IDbContext
{
    private readonly Lazy<NpgsqlConnection> _connectionLazy;
    private bool _disposed;

    public DbContext(string? connectionString)
    {
        _connectionLazy = new Lazy<NpgsqlConnection>(() =>
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        });
    }

    public IDbConnection Connection => _connectionLazy.Value;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            if (_connectionLazy is not { IsValueCreated: true, Value.State: ConnectionState.Open }) return;
            _connectionLazy.Value.Close();
            _connectionLazy.Value.Dispose();
        }

        _disposed = true;
    }

    ~DbContext()
    {
        Dispose(false);
    }
}