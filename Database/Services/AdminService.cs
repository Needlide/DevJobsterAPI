using Dapper;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.Models.Admin;
using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Database.Services;

public class AdminService(DbContext dbContext) : IAdminService
{
    public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM admins";
        return await connection.QueryAsync<Admin>(sql);
    }

    public async Task<Admin> GetAdminByIdAsync(int adminId)
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM admins WHERE admin_id = @adminId";
        return await connection.QuerySingleAsync<Admin>(sql, new { adminId });
    }

    public async Task<IEnumerable<Report>> GetAllReportsAsync()
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM reports";
        return await connection.QueryAsync<Report>(sql);
    }

    public async Task<Report> GetReportByIdAsync(int reportId)
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM reports WHERE report_id = @reportId";
        return await connection.QuerySingleAsync<Report>(sql, new { reportId });
    }

    public async Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM logs WHERE created_at => @startDate AND created_at <= @endDate";
        return await connection.QueryAsync<Log>(sql, new { startDate, endDate });
    }

    public async Task<IEnumerable<RegisteredAccount>> GetRegisteredAccountsAsync()
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM registered_accounts";
        return await connection.QueryAsync<RegisteredAccount>(sql);
    }

    public async Task<RegisteredAccount> GetRegisteredAccountByIdAsync(int accountId)
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM registered_accounts WHERE registered_account_id = @accountId";
        return await connection.QuerySingleAsync<RegisteredAccount>(sql, new { accountId });
    }

    public async Task<int> UpdateRegisteredAccountStatusAsync(Guid accountId, bool isChecked, UserType userType)
    {
        using var connection = dbContext.Connection;
        var sql = "UPDATE registered_accounts SET checked = @isChecked";

        sql = string.Concat(sql, userType == UserType.User ? "WHERE user_id = @accountId" : "WHERE recruiter_id = @accountId");

        return await connection.ExecuteAsync(sql, new { accountId, @checked = isChecked });
    }
}