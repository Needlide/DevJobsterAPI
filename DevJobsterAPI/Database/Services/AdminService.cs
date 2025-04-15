using Dapper;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Admin;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.Security;
using Npgsql;

namespace DevJobsterAPI.Database.Services;

public class AdminService(IDbContext dbContext) : IAdminService
{
    public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
    {
        try
        {
            const string sql = "SELECT * FROM admins";
            return await dbContext.Connection.QueryAsync<Admin>(sql);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<Admin?> GetAdminByIdAsync(Guid adminId)
    {
        try
        {
            const string sql = "SELECT * FROM admins WHERE admin_id = @adminId";
            return await dbContext.Connection.QuerySingleOrDefaultAsync<Admin>(sql, new { adminId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<Report>> GetAllReportsAsync()
    {
        try
        {
            const string sql = "SELECT * FROM reports";
            return await dbContext.Connection.QueryAsync<Report>(sql);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<Report?> GetReportByIdAsync(int reportId)
    {
        try
        {
            const string sql = "SELECT * FROM reports WHERE report_id = @reportId";
            return await dbContext.Connection.QuerySingleOrDefaultAsync<Report>(sql, new { reportId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            const string sql =
                "SELECT * FROM logs WHERE created_at >= @startDate AND created_at <= @endDate";
            return await dbContext.Connection.QueryAsync<Log>(sql, new { startDate, endDate });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<RegisteredAccount>> GetRegisteredAccountsAsync()
    {
        try
        {
            const string sql = """
                                   SELECT 
                                       registered_account_id AS RegisteredAccountId,
                                       user_id AS UserId,
                                       recruiter_id AS RecruiterId,
                                       checked AS IsChecked,
                                       created_at AS CreatedAt
                                   FROM registered_accounts
                               """;
            return await dbContext.Connection.QueryAsync<RegisteredAccount>(sql);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<RegisteredAccount?> GetRegisteredAccountByIdAsync(int accountId)
    {
        try
        {
            const string sql = """
                                   SELECT 
                                       registered_account_id AS RegisteredAccountId,
                                       user_id AS UserId,
                                       recruiter_id AS RecruiterId,
                                       checked AS IsChecked,
                                       created_at AS CreatedAt
                                   FROM registered_accounts
                                   WHERE registered_account_id = @accountId
                               """;
            return await dbContext.Connection
                .QuerySingleOrDefaultAsync<RegisteredAccount>(sql, new { accountId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> UpdateRegisteredAccountStatusAsync(RegisteredAccountUpdatedStatus updatedStatus)
    {
        try
        {
            var sql = """
                      UPDATE registered_accounts SET checked = @IsChecked
                      WHERE registered_account_id = @RegisteredAccountId
                      """;
            return await dbContext.Connection.ExecuteAsync(sql, updatedStatus);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }
}