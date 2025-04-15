using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.Admin;

public class AdminReport
{
    private AdminReport()
    {
    }

    public AdminReport(Guid adminId, int reportId)
    {
        AdminId = adminId;
        ReportId = reportId;
    }

    public AdminReport(Admin admin, Report report)
    {
        Admin = admin;
        Report = report;
        AdminId = admin.AdminId;
        ReportId = report.ReportId;
    }

    public Guid AdminId { get; init; }
    public int ReportId { get; init; }

    public Admin? Admin { get; init; }
    public Report? Report { get; init; }
}