using System.Data;
using Npgsql;

namespace DevJobsterAPI.Database;

public class DbContext(string connectionString) : IDisposable
{
    private NpgsqlConnection? _connection;

    public IDbConnection Connection
    {
        get
        {
            if (_connection is { State: ConnectionState.Open }) return _connection;
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            return _connection;
        }
    }

    public void Dispose()
    {
        if (_connection is not { State: ConnectionState.Open }) return;
        _connection.Close();
        _connection.Dispose();
    }
}