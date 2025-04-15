namespace DevJobsterAPI.DatabaseModels.Security;

public class Log
{
    private Log()
    {
    }

    public Log(string body, Guid adminId)
    {
        Body = body;
        AdminId = adminId;

        CreatedAt = DateTime.UtcNow;
    }

    public Log(string body, Admin.Admin admin)
    {
        Body = body;
        Admin = admin;
        AdminId = admin.AdminId;

        CreatedAt = DateTime.UtcNow;
    }

    public int LogId { get; init; }
    public Guid AdminId { get; init; }

    // Dapper will fill these properties
    // so telling compiler they're not null
    public string Body { get; init; } = null!;
    public DateTime CreatedAt { get; init; }

    public Admin.Admin? Admin { get; init; }
}