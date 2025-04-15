using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Admin;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
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
            return TypedResults.Ok(admins);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/admins/{adminId:guid}",
                async Task<Results<Ok<Admin>, NotFound>> (Guid adminId, IAdminService adminService) =>
                    await adminService.GetAdminByIdAsync(adminId) is { } admin
                        ? TypedResults.Ok(admin)
                        : TypedResults.NotFound())
            .RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/reports", async (IAdminService adminService) =>
        {
            var reports = await adminService.GetAllReportsAsync();
            return TypedResults.Ok(reports);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/reports/{reportId:int}",
            async Task<Results<Ok<Report>, NotFound>> (int reportId, IAdminService adminService) =>
                await adminService.GetReportByIdAsync(reportId) is { } report
                    ? TypedResults.Ok(report)
                    : TypedResults.NotFound()
        ).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/logs", async (DateTime startDate, DateTime endDate, IAdminService adminService) =>
        {
            var logs = await adminService.GetLogsByDateRangeAsync(startDate, endDate);
            return TypedResults.Ok(logs);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/accounts", async (IAdminService adminService) =>
        {
            var registeredAccounts = await adminService.GetRegisteredAccountsAsync();
            return TypedResults.Ok(registeredAccounts);
        }).RequireAuthorization("AdminOnly");

        adminGroup.MapGet("/accounts/{accountId:int}",
            async Task<Results<Ok<RegisteredAccount>, NotFound>> (int accountId, IAdminService adminService) =>
                await adminService.GetRegisteredAccountByIdAsync(accountId) is { } registeredAccount
                    ? TypedResults.Ok(registeredAccount)
                    : TypedResults.NotFound()
        ).RequireAuthorization("AdminOnly");

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