namespace DevJobsterAPI.Models.Security;
public class Log
{
    public int LogId { get; set; }
    public int AdminId { get; set; }
    public required string Body { get; set; }
    public DateTime CreatedAt { get; set; }

    public required Admin.Admin Admin { get; set; }
    
    public Log() {}

    public Log(string body, Admin.Admin admin)
    {
        Body = body;
        Admin = admin;
    }
}