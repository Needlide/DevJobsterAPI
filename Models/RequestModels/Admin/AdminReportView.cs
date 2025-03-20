using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Models.RequestModels.Admin;

public class AdminReportView(AdminView adminView, Report report)
{
    public required AdminView AdminView {get; set;} = adminView;
    public required Report Report {get; set;} = report;
}