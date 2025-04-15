using System.ComponentModel.DataAnnotations.Schema;
using DevJobsterAPI.Common;
using DevJobsterAPI.DatabaseModels.Admin;

namespace DevJobsterAPI.DatabaseModels.Security;

public class Report
{
    private Report()
    {
    }

    public Report(string title, string body, Guid reportObjectId,
        Guid? recruiterId = null, Guid? userId = null)
    {
        Title = title;
        Body = body;
        ReportObjectId = reportObjectId;
        RecruiterId = recruiterId;
        UserId = userId;

        CreatedAt = DateTime.UtcNow;
    }

    public Report(string title, string body, Guid reportObjectId,
        Recruiter.Recruiter? recruiter = null, User.User? user = null)
    {
        Title = title;
        Body = body;
        ReportObjectId = reportObjectId;
        User = user;
        Recruiter = recruiter;

        CreatedAt = DateTime.UtcNow;
    }

    public int ReportId { get; init; }
    public Guid? UserId { get; init; }
    public Guid? RecruiterId { get; init; }

    // Dapper will fill these properties
    // so telling compiler they're not null
    public string Title { get; init; } = null!;
    public string Body { get; init; } = null!;
    public Guid ReportObjectId { get; init; }
    public DateTime CreatedAt { get; init; }

    public User.User? User { get; init; }
    public Recruiter.Recruiter? Recruiter { get; init; }
    public List<AdminReport> AdminReports { get; init; } = [];

    [NotMapped] public Guid? ReporterId => UserId ?? RecruiterId;

    [NotMapped]
    public UserType? ReporterType
    {
        get
        {
            if (UserId.HasValue) return UserType.User;
            return RecruiterId.HasValue ? UserType.Recruiter : null;
        }
    }
}