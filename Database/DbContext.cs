using System.Data;
using Npgsql;

namespace DevJobsterAPI.Database;

public class DbContext : IDisposable
{
    private readonly Lazy<NpgsqlConnection> _connectionLazy;

    public DbContext(string connectionString)
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
        if (_connectionLazy is not { IsValueCreated: true, Value.State: ConnectionState.Open }) return;
        _connectionLazy.Value.Close();
        _connectionLazy.Value.Dispose();
    }
}