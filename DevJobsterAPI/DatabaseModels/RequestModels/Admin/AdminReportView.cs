using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Admin;

public record AdminReportView(AdminView AdminView, Report Report);