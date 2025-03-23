using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Models.RequestModels.Admin;

public class AdminView(string firstName, string lastName)
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public bool Role { get; set; } = true; // true = 1 (moderator), false = 0 (admin)

    public List<Log> Logs { get; set; } = [];
    public List<AdminReportView> AdminReports { get; set; } = [];
    public List<AdminRegisteredAccountView> AdminRegisteredAccounts { get; set; } = [];
}