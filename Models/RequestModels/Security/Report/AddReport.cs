using DevJobsterAPI.Common;

namespace DevJobsterAPI.Models.RequestModels.Security.Report;

public class AddReport
{
    public Guid SenderId { get; set; }
    public UserType SenderType { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required Guid ReportObjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}
