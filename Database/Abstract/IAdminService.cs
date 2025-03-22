using DevJobsterAPI.Models.Admin;
using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Database.Abstract;

public interface IAdminService
{
    Task<IEnumerable<Admin>> GetAllAdminsAsync();
    Task<Admin> GetAdminByIdAsync(int adminId);
    Task<IEnumerable<Report>> GetAllReportsAsync();
    Task<Report> GetReportByIdAsync(int reportId);
    Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<RegisteredAccount>> GetRegisteredAccountsAsync();
    Task<RegisteredAccount> GetRegisteredAccountByIdAsync(int accountId);
    Task UpdateRegisteredAccountStatusAsync(int accountId, bool isChecked);
}