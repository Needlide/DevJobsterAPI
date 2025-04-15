using System.Data;

namespace DevJobsterAPI.Database.Abstract;

public interface IDbContext : IDisposable
{
    IDbConnection Connection { get; }
}