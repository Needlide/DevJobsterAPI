namespace DevJobsterAPI.Models.RequestModels.Security.Report;

public class ReportView(string title, string body)
{
    public required string Title { get; set; } = title;
    public required string Body { get; set; } = body;
    public required Guid ReportObjectId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Models.User.User? User { get; set; }
    public Models.Recruiter.Recruiter? Recruiter { get; set; }
}