using DevJobsterAPI.ApiModels;
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
        var adminGroup = app.MapGroup("/api/admin").RequireCors("AllowFrontend");

        adminGroup.MapGet("/admins", async (IAdminService adminService) =>
        {
            var admins = await adminService.GetAllAdminsAsync();

            var adminViews = admins.Select(admin => new AdminView(
                admin.FirstName,
                admin.LastName,
                admin.Role
            ));

            return TypedResults.Ok(ApiResponseFactory.Success(adminViews));
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/admins/{adminId:guid}",
            async Task<Results<Ok<ApiResponse<AdminView>>, NotFound<ApiResponse<AdminView>>>> (Guid adminId,
                IAdminService adminService) =>
            {
                var admin = await adminService.GetAdminByIdAsync(adminId);

                if (admin is null)
                    return TypedResults.NotFound(ApiResponseFactory.Fail<AdminView>(
                        "Admin not found", "NOT_FOUND"));

                var adminView = new AdminView(admin.FirstName, admin.LastName, admin.Role);

                return TypedResults.Ok(ApiResponseFactory.Success(adminView));
            }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/reports", async (IAdminService adminService) =>
        {
            var reports = await adminService.GetAllReportsAsync();

            var reportViews = reports.Select(report => new ReportView(
                report.Title, report.Body, report.ReportObjectId, report.CreatedAt));

            return TypedResults.Ok(ApiResponseFactory.Success(reportViews));
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/reports/{reportId:int}",
            async Task<Results<Ok<ApiResponse<ReportView>>, NotFound<ApiResponse<ReportView>>>> (int reportId,
                IAdminService adminService) =>
            {
                var report = await adminService.GetReportByIdAsync(reportId);

                if (report is null)
                    return TypedResults.NotFound(ApiResponseFactory.Fail<ReportView>(
                        "Report not found", "NOT_FOUND"));

                var reportView = new ReportView(report.Title, report.Body, report.ReportObjectId, report.CreatedAt);

                return TypedResults.Ok(ApiResponseFactory.Success(reportView));
            }).RequireAuthorization("AdminOnly");

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

            return TypedResults.Ok(ApiResponseFactory.Success(logViews));
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

            return TypedResults.Ok(ApiResponseFactory.Success(accountViews));
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/accounts/{accountId:int}",
            async Task<Results<Ok<ApiResponse<RegisteredAccountShortView>>,
                NotFound<ApiResponse<RegisteredAccountShortView>>>> (int accountId, IAdminService adminService) =>
            {
                var registeredAccount = await adminService.GetRegisteredAccountByIdAsync(accountId);

                if (registeredAccount == null ||
                    (registeredAccount.Recruiter == null && registeredAccount.User == null))
                    return TypedResults.NotFound(ApiResponseFactory.Fail<RegisteredAccountShortView>(
                        "Account not found", "NOT_FOUND"));

                RegisteredAccountShortView registeredAccountShortView;

                if (registeredAccount.Recruiter != null)
                    registeredAccountShortView = new RegisteredAccountShortView(
                        registeredAccount.Recruiter.FirstName,
                        registeredAccount.Recruiter.LastName,
                        UserType.Recruiter,
                        registeredAccount.Recruiter.RecruiterId,
                        registeredAccount.CreatedAt);
                else // User is not null based on the check above
                    registeredAccountShortView = new RegisteredAccountShortView(
                        registeredAccount.User!.FirstName,
                        registeredAccount.User!.LastName,
                        UserType.User,
                        registeredAccount.User!.UserId,
                        registeredAccount.CreatedAt);

                return TypedResults.Ok(ApiResponseFactory.Success(registeredAccountShortView));
            }).RequireAuthorization("AdminOnly");

        adminGroup.MapPut("/accounts/status",
            async Task<Results<Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>>> (
                RegisteredAccountUpdatedStatus status, IAdminService adminService) =>
            {
                var result = await adminService.UpdateRegisteredAccountStatusAsync(status);

                if (result > 0)
                    return TypedResults.Ok(
                        ApiResponseFactory.Success<object>(null, "Account status updated successfully"));

                return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                    "Account not found or status update failed", "NOT_FOUND"));
            }).RequireAuthorization("AdminOnly");

        return app;
    }
}