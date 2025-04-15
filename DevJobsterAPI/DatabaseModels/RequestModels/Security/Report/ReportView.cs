namespace DevJobsterAPI.DatabaseModels.RequestModels.Security.Report;

public record ReportView(string Title, string Body, Guid ReportObjectId, DateTime CreatedAt);