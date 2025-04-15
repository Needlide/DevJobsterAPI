using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.Admin;

public class Admin
{
    private Admin()
    {
    }

    public Admin(string firstName, string lastName, string email, bool role = true) // role = true - moderator
    {
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Email = email;
    }

    public Guid AdminId { get; init; } = Guid.NewGuid();
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public bool Role { get; init; } // true = 1 (moderator), false = 0 (admin)
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public List<Log> Logs { get; init; } = [];
    public List<AdminReport> AdminReports { get; init; } = [];
    public List<AdminRegisteredAccount> AdminRegisteredAccounts { get; init; } = [];
}