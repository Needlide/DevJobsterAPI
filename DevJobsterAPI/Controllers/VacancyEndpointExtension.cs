using System.Security.Claims;
using DevJobsterAPI.ApiModels;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.User;
using DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;
using DevJobsterAPI.DatabaseModels.Vacancy;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class VacancyEndpointExtension
{
    public static WebApplication MapVacancyEndpoint(this WebApplication app)
{
    var vacancyGroup = app.MapGroup("/api/vacancies")
        .WithTags("Vacancies"); // Swagger grouping

    vacancyGroup.MapGet("/",
        async (IUserSpaceService userSpaceService, IUserManagementService userManagementService) =>
        {
            var vacanciesEnumerable = await userSpaceService.GetAllVacanciesAsync();

            var vacancies = vacanciesEnumerable.ToList();
            var recruiterIds = vacancies
                .Where(v => v.Recruiter == null)
                .Select(v => v.RecruiterId)
                .Distinct()
                .ToList();

            var recruiters = await userManagementService.GetRecruitersByIdsAsync(recruiterIds);

            var recruiterDict = recruiters.ToDictionary(r => r.RecruiterId);

            var vacancyViews = vacancies.Select(v =>
            {
                var recruiter = v.Recruiter ?? recruiterDict.GetValueOrDefault(v.RecruiterId);

                var recruiterView = recruiter != null ? new RecruiterView(
                    recruiter.FirstName,
                    recruiter.LastName,
                    recruiter.Notes,
                    recruiter.Company,
                    recruiter.PhoneNumber) : null;

                return new VacancyView(
                    v.VacancyId,
                    v.Title,
                    v.Description,
                    v.Requirements,
                    v.CompanyWebsite,
                    v.TypeOfJob,
                    v.Location,
                    v.Country,
                    recruiterView);
            }).ToList();

            return TypedResults.Ok(ApiResponseFactory.Success(vacancyViews));
        }).RequireAuthorization("UserAndAdminOnly");

    vacancyGroup.MapGet("/{vacancyId:guid}",
            async Task<Results<Ok<ApiResponse<VacancyView>>, NotFound<ApiResponse<VacancyView>>>> (
                Guid vacancyId, 
                IUserSpaceService userSpaceService,
                IUserManagementService userManagementService) =>
            {
                var v = await userSpaceService.GetVacancyByIdAsync(vacancyId);

                if (v is null)
                    return TypedResults.NotFound(ApiResponseFactory.Fail<VacancyView>(
                        "Vacancy not found", "NOT_FOUND"));

                var recruiter = v.Recruiter ?? await userManagementService.GetRecruiterByIdAsync(v.RecruiterId);
                var recruiterView = recruiter != null ? new RecruiterView(
                    recruiter.FirstName,
                    recruiter.LastName,
                    recruiter.Notes,
                    recruiter.Company,
                    recruiter.PhoneNumber) : null;

                var vacancyView = new VacancyView(
                    v.VacancyId,
                    v.Title, 
                    v.Description, 
                    v.Requirements, 
                    v.CompanyWebsite,
                    v.TypeOfJob, 
                    v.Location, 
                    v.Country,
                    recruiterView);

                return TypedResults.Ok(ApiResponseFactory.Success(vacancyView));
            })
        .RequireAuthorization();

    vacancyGroup.MapPost("/",
            async Task<Results<Created<ApiResponse<AddVacancy>>, BadRequest<ApiResponse<AddVacancy>>>> (
                AddVacancy vacancy,
                ClaimsPrincipal user,
                IUserSpaceService userSpaceService) =>
            {
                var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<AddVacancy>(
                        "User ID not found in claims", "INVALID_USER"));

                var recruiterId = Guid.Parse(senderId);
                
                var result = await userSpaceService.CreateVacancyAsync(vacancy, recruiterId);
                
                if (result > 0)
                    return TypedResults.Created(
                        $"/api/vacancies/{vacancy.VacancyId}", 
                        ApiResponseFactory.Success(vacancy, "Vacancy created successfully"));
                else
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<AddVacancy>(
                        "Failed to create vacancy", "CREATION_FAILED"));
            })
        .WithValidation<Vacancy>()
        .RequireAuthorization("RecruiterOnly");

    vacancyGroup.MapPut("/{vacancyId:guid}",
            async Task<Results<BadRequest<ApiResponse<object>>, Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>>> (
                Guid vacancyId, 
                UpdateVacancy vacancy,
                IUserSpaceService userSpaceService) =>
            {
                if (vacancyId != vacancy.VacancyId) 
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<object>(
                        "Vacancy ID in route does not match ID in request body", "ID_MISMATCH"));
                
                var result = await userSpaceService.UpdateVacancyAsync(vacancy);
                
                if (result > 0)
                    return TypedResults.Ok(ApiResponseFactory.Success<object>(null, "Vacancy updated successfully"));
                else
                    return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                        "Vacancy not found", "NOT_FOUND"));
            })
        .WithValidation<UpdateVacancy>()
        .RequireAuthorization("RecruiterOnly");

    vacancyGroup.MapDelete("/{vacancyId:guid}",
            async Task<Results<Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>>> (
                Guid vacancyId, 
                IUserSpaceService userSpaceService) =>
            {
                var result = await userSpaceService.DeleteVacancyAsync(vacancyId);
                
                if (result > 0)
                    return TypedResults.Ok(ApiResponseFactory.Success<object>(null, "Vacancy deleted successfully"));
                else
                    return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                        "Vacancy not found", "NOT_FOUND"));
            })
        .RequireAuthorization("RecruiterAndAdminOnly");

    vacancyGroup.MapGet("/{vacancyId:guid}/applications",
        async Task<Results<Ok<ApiResponse<List<UserApplicationView>>>, NotFound<ApiResponse<List<UserApplicationView>>>>> (
            Guid vacancyId, 
            IUserSpaceService userSpaceService, 
            IUserManagementService userManagementService) =>
        {
            // First check if vacancy exists
            var vacancy = await userSpaceService.GetVacancyByIdAsync(vacancyId);
            if (vacancy == null)
                return TypedResults.NotFound(ApiResponseFactory.Fail<List<UserApplicationView>>(
                    "Vacancy not found", "NOT_FOUND"));
                
            var vacancyApplications = await userSpaceService.GetApplicationsByVacancyIdAsync(vacancyId);

            var vacancyApplicationViews = new List<UserApplicationView>();

            foreach (var va in vacancyApplications)
            {
                var applicationUser = va.User ?? await userManagementService.GetUserByIdAsync(va.UserId);

                if (applicationUser is null) continue;

                vacancyApplicationViews.Add(new UserApplicationView(
                    applicationUser.FirstName,
                    applicationUser.LastName,
                    applicationUser.Role,
                    applicationUser.Location,
                    applicationUser.YearsOfExperience,
                    applicationUser.EnglishLevel));
            }

            return TypedResults.Ok(ApiResponseFactory.Success(vacancyApplicationViews));
        }).RequireAuthorization("RecruiterOnly");

    vacancyGroup.MapGet("/applications",
        async Task<Results<Ok<ApiResponse<IEnumerable<VacancyView>>>, BadRequest<ApiResponse<IEnumerable<VacancyView>>>>> (
            ClaimsPrincipal user,
            IUserSpaceService userSpaceService,
            IUserManagementService userManagementService) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return TypedResults.BadRequest(ApiResponseFactory.Fail<IEnumerable<VacancyView>>(
                    "User ID not found in claims", "INVALID_USER"));

            var vacancies = await userSpaceService.GetVacanciesByUserIdAsync(userId);

            var vacancyViews = new List<VacancyView>();

            foreach (var v in vacancies)
            {
                var recruiter = v.Recruiter ?? await userManagementService.GetRecruiterByIdAsync(v.RecruiterId);

                var recruiterView = recruiter != null ? new RecruiterView(
                    recruiter.FirstName,
                    recruiter.LastName,
                    recruiter.Notes,
                    recruiter.Company,
                    recruiter.PhoneNumber) : null;
                
                vacancyViews.Add(new VacancyView(
                    v.VacancyId,
                    v.Title,
                    v.Description,
                    v.Requirements,
                    v.CompanyWebsite,
                    v.TypeOfJob,
                    v.Location,
                    v.Country,
                    recruiterView));
            }

            return TypedResults.Ok(ApiResponseFactory.Success(vacancyViews.AsEnumerable()));
        }).RequireAuthorization("UserOnly");
    
    return app;
}
}