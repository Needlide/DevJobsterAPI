using System.Security.Claims;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;
using DevJobsterAPI.DatabaseModels.Vacancy;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class ApplicationEndpointExtension
{
    public static WebApplication MapApplicationEndpoint(this WebApplication app)
    {
        var applicationGroup = app.MapGroup("/api/applications");

        applicationGroup.MapPost("/",
                async Task<Results<Created<AddApplication>, BadRequest>> (
                    AddApplication addApplication,
                    ClaimsPrincipal user,
                    IUserSpaceService userSpaceService) =>
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (userId is null)
                    {
                        return TypedResults.BadRequest();
                    }
                    
                    var applicationId = await userSpaceService.CreateApplicationAsync(Guid.Parse(userId),  addApplication);
                         return applicationId > 0
                        ? TypedResults.Created($"/api/applications/{applicationId}", addApplication)
                        : TypedResults.BadRequest();
                })
            .WithValidation<Application>()
            .RequireAuthorization("UsersOnly");

        return app;
    }
}