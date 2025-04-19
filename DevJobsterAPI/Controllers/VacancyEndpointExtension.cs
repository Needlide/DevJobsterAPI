using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
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
                var vacancies = await userSpaceService.GetAllVacanciesAsync();

                var vacancyViews = vacancies.Select(async v => new VacancyView(
                    v.Title, v.Description, v.Requirements, v.CompanyWebsite, v.TypeOfJob, v.Location, v.Country,
                    v.Recruiter ?? await userManagementService.GetRecruiterByIdAsync(v.RecruiterId)));

                return TypedResults.Ok(vacancyViews);
            }).RequireAuthorization("UserAndAdminOnly");

        vacancyGroup.MapGet("/{vacancyId:guid}",
                async Task<Results<Ok<VacancyView>, NotFound>> (Guid vacancyId, IUserSpaceService userSpaceService,
                    IUserManagementService userManagementService) =>
                {
                    var v = await userSpaceService.GetVacancyByIdAsync(vacancyId);

                    if (v is null)
                        return TypedResults.NotFound();

                    var vacancyView = new VacancyView(v.Title, v.Description, v.Requirements, v.CompanyWebsite,
                        v.TypeOfJob, v.Location, v.Country,
                        v.Recruiter ?? await userManagementService.GetRecruiterByIdAsync(v.RecruiterId));

                    return TypedResults.Ok(vacancyView);
                })
            .RequireAuthorization();

        vacancyGroup.MapPost("/",
                async Task<Results<Created<AddVacancy>, BadRequest>> (AddVacancy vacancy,
                        IUserSpaceService userSpaceService) =>
                    await userSpaceService.CreateVacancyAsync(vacancy) > 0
                        ? TypedResults.Created($"/api/vacancies/{vacancy.VacancyId}", vacancy)
                        : TypedResults.BadRequest())
            .WithValidation<Vacancy>()
            .RequireAuthorization("RecruiterOnly");

        vacancyGroup.MapPut("/{vacancyId:guid}",
                async Task<Results<BadRequest, NoContent, NotFound>> (Guid vacancyId, UpdateVacancy vacancy,
                    IUserSpaceService userSpaceService) =>
                {
                    if (vacancyId != vacancy.VacancyId) return TypedResults.BadRequest();
                    return await userSpaceService.UpdateVacancyAsync(vacancy) > 0
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound();
                })
            .WithValidation<UpdateVacancy>()
            .RequireAuthorization("RecruiterOnly");

        vacancyGroup.MapDelete("/{vacancyId:guid}",
                async Task<Results<NoContent, NotFound>> (Guid vacancyId, IUserSpaceService userSpaceService) =>
                    await userSpaceService.DeleteVacancyAsync(vacancyId) > 0
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound())
            .RequireAuthorization("RecruiterAndAdminOnly");

        vacancyGroup.MapGet("/{vacancyId:guid}/applications",
            async (Guid vacancyId, IUserSpaceService userSpaceService, IUserManagementService userManagementService) =>
            {
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

                return TypedResults.Ok(vacancyApplicationViews);
            }).RequireAuthorization("RecruiterOnly");

        return app;
    }
}