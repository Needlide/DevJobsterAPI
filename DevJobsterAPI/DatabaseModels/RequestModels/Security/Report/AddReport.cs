using DevJobsterAPI.Common;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Security.Report;

public record AddReport(
    Guid SenderId,
    UserType SenderType,
    string Title,
    string Body,
    Guid ReportObjectId,
    DateTime CreatedAt);