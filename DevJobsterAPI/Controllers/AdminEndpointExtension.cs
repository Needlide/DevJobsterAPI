using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.RequestModels.Admin;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.RequestModels.Security.Report;
using DevJobsterAPI.DatabaseModels.Security;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class AdminEndpointExtension
{
    public static WebApplication MapAdminEndpoint(this WebApplication app)
    {
        var adminGroup = app.MapGroup("/api/admin");

        adminGroup.MapGet("/admins", async (IAdminService adminService) =>
        {
            var admins = await adminService.GetAllAdminsAsync();

            var adminViews = admins.Select(admin => new AdminView(
                admin.FirstName,
                admin.LastName,
                admin.Role
            ));

            return TypedResults.Ok(adminViews);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/admins/{adminId:guid}",
                async Task<Results<Ok<AdminView>, NotFound>> (Guid adminId, IAdminService adminService) =>
                {
                    var admin = await adminService.GetAdminByIdAsync(adminId);

                    if (admin is null)
                        return TypedResults.NotFound();

                    var adminView = new AdminView(admin.FirstName, admin.LastName, admin.Role);

                    return TypedResults.Ok(adminView);
                })
            .RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/reports", async (IAdminService adminService) =>
        {
            var reports = await adminService.GetAllReportsAsync();

            var reportViews = reports.Select(report => new ReportView(
                report.Title, report.Body, report.ReportObjectId, report.CreatedAt));

            return TypedResults.Ok(reportViews);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/reports/{reportId:int}",
            async Task<Results<Ok<ReportView>, NotFound>> (int reportId, IAdminService adminService) =>
            {
                var report = await adminService.GetReportByIdAsync(reportId);

                if (report is null)
                    return TypedResults.NotFound();

                var reportView = new ReportView(report.Title, report.Body, report.ReportObjectId, report.CreatedAt);

                return TypedResults.Ok(reportView);
            }
        ).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/logs", async (DateTime startDate, DateTime endDate, IAdminService adminService) =>
        {
            var logs = await adminService.GetLogsByDateRangeAsync(startDate, endDate);
            
            var logViews = logs.Select(log =>
            {
                if (log.Admin != null)
                    return new LogView(log.Body,
                        new AdminView(log.Admin.FirstName, log.Admin.LastName, log.Admin.Role), log.CreatedAt);
                return new LogView(log.Body, null, log.CreatedAt);
            });

            return TypedResults.Ok(logViews);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/accounts", async (IAdminService adminService) =>
        {
            var registeredAccounts = await adminService.GetRegisteredAccountsAsync();

            var accountViews = registeredAccounts
                .Where(acc => acc.User is not null || acc.Recruiter is not null)
                .Select<RegisteredAccount, RegisteredAccountShortView>(acc =>
                {
                    if (acc.User is not null)
                        return new RegisteredAccountShortView(
                            acc.User.FirstName,
                            acc.User.LastName,
                            UserType.User,
                            acc.User.UserId,
                            acc.CreatedAt
                        );
                    if (acc.Recruiter is not null)
                        return new RegisteredAccountShortView(
                            acc.Recruiter.FirstName,
                            acc.Recruiter.LastName,
                            UserType.Recruiter,
                            acc.Recruiter.RecruiterId,
                            acc.CreatedAt
                        );

                    throw new InvalidOperationException("RegisteredAccount must have either a User or a Recruiter.");
                })
                .ToList();

            return TypedResults.Ok(accountViews);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/accounts/{accountId:int}",
            async Task<Results<Ok<RegisteredAccountShortView>, NotFound>> (int accountId, IAdminService adminService) =>
            {
                var registeredAccount = await adminService.GetRegisteredAccountByIdAsync(accountId);

                switch (registeredAccount)
                {
                    case { Recruiter: not null }:
                    {
                        var registeredAccountShortView = new RegisteredAccountShortView(
                            registeredAccount.Recruiter.FirstName,
                            registeredAccount.Recruiter.LastName,
                            UserType.Recruiter,
                            registeredAccount.Recruiter.RecruiterId,
                            registeredAccount.CreatedAt);

                        return TypedResults.Ok(registeredAccountShortView);
                    }
                    case { User: not null }:
                    {
                        var registeredAccountShortView = new RegisteredAccountShortView(
                            registeredAccount.User.FirstName,
                            registeredAccount.User.LastName,
                            UserType.User,
                            registeredAccount.User.UserId,
                            registeredAccount.CreatedAt);

                        return TypedResults.Ok(registeredAccountShortView);
                    }
                    default:
                        return TypedResults.NotFound();
                }
            }).RequireAuthorization("AdminOnly");

        adminGroup.MapPut("/accounts/status",
                async Task<Results<NoContent, NotFound>> (RegisteredAccountUpdatedStatus status,
                        IAdminService adminService) =>
                    await adminService.UpdateRegisteredAccountStatusAsync(status) > 0
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound())
            .RequireAuthorization("AdminOnly");

        return app;
    }
}