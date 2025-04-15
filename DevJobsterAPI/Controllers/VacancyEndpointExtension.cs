using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
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

        vacancyGroup.MapGet("/", async (IUserSpaceService userSpaceService) =>
        {
            var vacancies = await userSpaceService.GetAllVacanciesAsync();
            return TypedResults.Ok(vacancies);
        }).RequireAuthorization("UserAndAdminOnly");

        vacancyGroup.MapGet("/{vacancyId:guid}",
                async Task<Results<Ok<Vacancy>, NotFound>> (Guid vacancyId, IUserSpaceService userSpaceService) =>
                    await userSpaceService.GetVacancyByIdAsync(vacancyId) is { } vacancy
                        ? TypedResults.Ok(vacancy)
                        : TypedResults.NotFound())
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
            async (Guid vacancyId, IUserSpaceService userSpaceService) =>
            {
                var vacancyApplications = await userSpaceService.GetApplicationsByVacancyIdAsync(vacancyId);
                return TypedResults.Ok(vacancyApplications);
            }).RequireAuthorization("RecruiterOnly");

        return app;
    }
}