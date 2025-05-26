using System.Security.Claims;
using DevJobsterAPI.ApiModels;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;
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

        return TypedResults.Ok(ApiResponseFactory.Success(recruiterViews));
    }).RequireAuthorization("AdminOnly");

    recruiterGroup.MapGet("/me",
            async Task<Results<Ok<ApiResponse<RecruiterView>>, NotFound<ApiResponse<RecruiterView>>, BadRequest<ApiResponse<RecruiterView>>>> (
                ClaimsPrincipal user,
                IUserManagementService userService) =>
            {
                var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<RecruiterView>(
                        "User ID not found in claims", "INVALID_USER"));

                var recruiterId = Guid.Parse(senderId);
                
                var recruiter = await userService.GetRecruiterByIdAsync(recruiterId);

                if (recruiter == null)
                    return TypedResults.NotFound(ApiResponseFactory.Fail<RecruiterView>(
                        "Recruiter not found", "NOT_FOUND"));

                var recruiterView = new RecruiterView(recruiter.FirstName, recruiter.LastName, recruiter.Notes,
                    recruiter.Company, recruiter.PhoneNumber);

                return TypedResults.Ok(ApiResponseFactory.Success(recruiterView));
            })
        .RequireAuthorization("RecruiterAndAdminOnly");

    recruiterGroup.MapPost("/",
            async Task<Results<Created<ApiResponse<Recruiter>>, BadRequest<ApiResponse<Recruiter>>>> (
                LoginRegisterModel registration,
                IUserManagementService userService) =>
            {
                var recruiter = new Recruiter(string.Empty, string.Empty, registration.Email, string.Empty,
                    string.Empty);

                var userAuthentication = new UserAuthentication(recruiter.RecruiterId, registration.Password);

                var recruiterRegistration = new RecruiterRegistration(recruiter, userAuthentication);

                var result = await userService.CreateRecruiterAsync(recruiterRegistration);
                
                if (result > 0)
                    return TypedResults.Created(
                        $"/api/recruiters/{recruiterRegistration.Recruiter.RecruiterId}",
                        ApiResponseFactory.Success(recruiter, "Recruiter created successfully"));
                else
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<Recruiter>(
                        "Failed to create recruiter", "CREATION_FAILED"));
            })
        .WithValidation<RecruiterRegistration>().AllowAnonymous();

    recruiterGroup.MapPut("/me",
            async Task<Results<Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>, BadRequest<ApiResponse<object>>>> (
                RecruiterUpdate recruiter,
                ClaimsPrincipal user,
                IUserManagementService userService) =>
            {
                var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<object>(
                        "User ID not found in claims", "INVALID_USER"));

                var recruiterId = Guid.Parse(senderId);

                var result = await userService.UpdateRecruiterAsync(recruiterId, recruiter);
                
                if (result > 0)
                    return TypedResults.Ok(ApiResponseFactory.Success<object>(null, "Recruiter updated successfully"));
                else
                    return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                        "Recruiter not found", "NOT_FOUND"));
            })
        .WithValidation<Recruiter>()
        .RequireAuthorization("RecruiterOnly");

    recruiterGroup.MapPost("/reset-password",
            async Task<Results<Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>, BadRequest<ApiResponse<object>>>> (
                ClaimsPrincipal claimsPrincipal, 
                UserAuthentication userAuth,
                IUserManagementService userService) =>
            {
                var senderId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<object>(
                        "User ID not found in claims", "INVALID_USER"));

                var userId = Guid.Parse(senderId);
                
                var result = await userService.ResetPasswordAsync(userId, userAuth);
                
                if (result > 0)
                    return TypedResults.Ok(ApiResponseFactory.Success<object>(null, "Password reset successfully"));
                else
                    return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                        "User not found or password reset failed", "RESET_FAILED"));
            })
        .WithValidation<UserAuthentication>()
        .RequireAuthorization("RecruiterOnly");

    recruiterGroup.MapDelete("/me",
            async Task<Results<Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>, BadRequest<ApiResponse<object>>>> (
                ClaimsPrincipal user,
                IUserManagementService userService) =>
            {
                var recruiterIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (recruiterIdStr is null || !Guid.TryParse(recruiterIdStr, out var recruiterId))
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<object>(
                        "Invalid user ID in claims", "INVALID_USER"));

                var result = await userService.DeleteRecruiterAsync(recruiterId);
                
                if (result > 0)
                    return TypedResults.Ok(ApiResponseFactory.Success<object>(null, "Recruiter deleted successfully"));
                else
                    return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                        "Recruiter not found", "NOT_FOUND"));
            })
        .RequireAuthorization("RecruiterOnly");

    recruiterGroup.MapGet("/vacancies", async Task<Results<Ok<ApiResponse<List<VacancyView>>>, BadRequest<ApiResponse<List<VacancyView>>>>> (
            ClaimsPrincipal claimsPrincipal, IUserSpaceService userService, IUserManagementService userManagementService
        ) =>
        {
            var senderId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
            if (senderId is null)
                return TypedResults.BadRequest(ApiResponseFactory.Fail<List<VacancyView>>(
                    "User ID not found in claims", "INVALID_USER"));

            var recruiterId = Guid.Parse(senderId);

            var recruiter = await userManagementService.GetRecruiterByIdAsync(recruiterId);
            var recruiterVacancies = await userService.GetVacanciesByRecruiterIdAsync(recruiterId);
            
            var vacancyViews = await Task.WhenAll(recruiterVacancies.Select(v => 
            {
                var recruiterView = recruiter != null ? new RecruiterView(
                    recruiter.FirstName,
                    recruiter.LastName,
                    recruiter.Email,
                    recruiter.Company,
                    recruiter.PhoneNumber) : null;

                return Task.FromResult(new VacancyView(
                    v.VacancyId,
                    v.Title, 
                    v.Description, 
                    v.Requirements, 
                    v.CompanyWebsite, 
                    v.TypeOfJob, 
                    v.Location, 
                    v.Country,
                    recruiterView));
            }));

            return TypedResults.Ok(ApiResponseFactory.Success(vacancyViews.ToList()));
        }).RequireAuthorization("RecruiterOnly");
    
    return app;
}
}