using DevJobsterAPI.DatabaseModels.Admin;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.Database.Abstract;

public interface IAdminService
{
    Task<IEnumerable<Admin>> GetAllAdminsAsync();
    Task<Admin?> GetAdminByIdAsync(Guid adminId);
    Task<IEnumerable<Report>> GetAllReportsAsync();
    Task<Report?> GetReportByIdAsync(int reportId);
    Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<RegisteredAccount>> GetRegisteredAccountsAsync();
    Task<RegisteredAccount?> GetRegisteredAccountByIdAsync(int accountId);
    Task<int> UpdateRegisteredAccountStatusAsync(RegisteredAccountUpdatedStatus updatedStatus);
}