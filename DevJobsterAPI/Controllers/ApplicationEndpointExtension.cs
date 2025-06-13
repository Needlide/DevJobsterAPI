using System.Security.Claims;
using DevJobsterAPI.ApiModels;
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
        var applicationGroup = app.MapGroup("/api/applications").RequireCors("AllowFrontend");

        applicationGroup.MapPost("/",
                async Task<Results<Created<ApiResponse<AddApplication>>, BadRequest<ApiResponse<AddApplication>>>> (
                    AddApplication addApplication,
                    ClaimsPrincipal user,
                    IUserSpaceService userSpaceService) =>
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (userId is null)
                        return TypedResults.BadRequest(ApiResponseFactory.Fail<AddApplication>(
                            "User ID not found in claims", "INVALID_USER"));

                    var applicationId = await userSpaceService.CreateApplicationAsync(Guid.Parse(userId), addApplication);
        
                    if (applicationId > 0)
                        return TypedResults.Created(
                            $"/api/applications/{applicationId}", 
                            ApiResponseFactory.Success(addApplication, "Application created successfully"));
                    else
                        return TypedResults.BadRequest(ApiResponseFactory.Fail<AddApplication>(
                            "Failed to create application", "CREATION_FAILED"));
                })
            .WithValidation<Application>()
            .RequireAuthorization("UserOnly");

        return app;
    }
}