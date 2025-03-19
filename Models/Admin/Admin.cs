using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Models.Admin;

public class Admin
{
    public int AdminId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public bool Role { get; set; } = true; // true = 1 (moderator), false = 0 (admin)
    public DateTime CreatedAt { get; set; }
    
    public List<Log> Logs { get; set; } = [];
    public List<AdminReport> AdminReports { get; set; } = [];
    public List<AdminRegisteredAccount> AdminRegisteredAccounts { get; set; } = [];
    
    public Admin() {}

    public Admin(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}