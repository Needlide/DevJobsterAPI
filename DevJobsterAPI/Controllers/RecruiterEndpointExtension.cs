using System.Security.Claims;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.Security;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class RecruiterEndpointExtension
{
    public static WebApplication MapRecruiterEndpoint(this WebApplication app)
    {
        var recruiterGroup = app.MapGroup("/api/recruiters")
            .WithTags("Recruiters"); // Swagger grouping

        recruiterGroup.MapGet("/", async (IUserManagementService userService) =>
        {
            var recruiters = await userService.GetAllRecruitersAsync();

            var recruiterViews = recruiters.Select(r => new RecruiterView(
                r.FirstName, r.LastName, r.Notes, r.Company, r.PhoneNumber));
            
            return TypedResults.Ok(recruiterViews);
        }).RequireAuthorization("AdminOnly");

        recruiterGroup.MapGet("/{recruiterId:guid}",
                async Task<Results<Ok<RecruiterView>, NotFound>> (Guid recruiterId, IUserManagementService userService) =>
                {
                    var recruiter = await userService.GetRecruiterByIdAsync(recruiterId);

                    if (recruiter == null)
                        return TypedResults.NotFound();
                    
                    var recruiterView = new RecruiterView(recruiter.FirstName, recruiter.LastName, recruiter.Notes, recruiter.Company, recruiter.PhoneNumber);
                    
                    return TypedResults.Ok(recruiterView);
                })
            .RequireAuthorization("RecruiterAndAdminOnly");

        recruiterGroup.MapPost("/",
                async Task<Results<Created<Recruiter>, BadRequest>> (LoginRegisterModel registration,
                    IUserManagementService userService) =>
                {
                    var recruiter = new Recruiter(string.Empty, string.Empty, registration.Email, string.Empty,
                        string.Empty);

                    var userAuthentication = new UserAuthentication(recruiter.RecruiterId, registration.Password);

                    var recruiterRegistration = new RecruiterRegistration(recruiter, userAuthentication);

                    return await userService.CreateRecruiterAsync(recruiterRegistration) > 0
                        ? TypedResults.Created($"/api/recruiters/{recruiterRegistration.Recruiter.RecruiterId}",
                            recruiter)
                        : TypedResults.BadRequest();
                })
            .WithValidation<RecruiterRegistration>();

        recruiterGroup.MapPut("/{recruiterId:guid}",
                async Task<Results<NoContent, NotFound, BadRequest>> (
                    RecruiterUpdate recruiter,
                    ClaimsPrincipal user,
                    IUserManagementService userService) =>
                {
                    var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (senderId is null)
                        return TypedResults.BadRequest();

                    var recruiterId = Guid.Parse(senderId);

                    return await userService.UpdateRecruiterAsync(recruiterId, recruiter) > 0
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound();
                })
            .WithValidation<Recruiter>()
            .RequireAuthorization("RecruiterOnly");

        recruiterGroup.MapPost("/reset-password",
                async Task<Results<NoContent, NotFound>> (UserAuthentication userAuth,
                    IUserManagementService userService) => await userService.ResetPasswordAsync(userAuth) > 0
                    ? TypedResults.NoContent()
                    : TypedResults.NotFound())
            .WithValidation<UserAuthentication>()
            .RequireAuthorization("RecruiterOnly");

        recruiterGroup.MapDelete("/{recruiterId:guid}",
                async Task<Results<NoContent, NotFound>> (Guid recruiterId, IUserManagementService userService) =>
                    await userService.DeleteRecruiterAsync(recruiterId) > 0
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound())
            .RequireAuthorization("RecruiterAndAdminOnly");

        return app;
    }
}