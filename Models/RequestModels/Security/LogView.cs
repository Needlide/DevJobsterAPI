namespace DevJobsterAPI.Models.RequestModels.Security;
public class LogView(string body, Models.Admin.Admin admin)
{
    public required string Body { get; set; } = body;
    public DateTime CreatedAt { get; set; }

    public required Models.Admin.Admin Admin { get; set; } = admin;
}