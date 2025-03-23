using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Models.Admin;

public class AdminReport
{
    public AdminReport()
    {
    }

    public AdminReport(Admin admin, Report report)
    {
        Admin = admin;
        Report = report;
    }

    public int AdminId { get; set; }
    public int ReportId { get; set; }

    public required Admin Admin { get; set; }
    public required Report Report { get; set; }
}