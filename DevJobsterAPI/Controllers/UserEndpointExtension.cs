using System.Security.Claims;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.RequestModels.User;
using DevJobsterAPI.DatabaseModels.Security;
using DevJobsterAPI.DatabaseModels.User;
using DevJobsterAPI.DatabaseModels.Vacancy;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class UserEndpointExtension
{
    public static WebApplication MapUserEndpoint(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/users")
            .WithTags("Users"); // Swagger grouping

        userGroup.MapGet("/", async (IUserManagementService service) =>
        {
            var users = await service.GetAllUsersAsync();
            return TypedResults.Ok(users);
        }).RequireAuthorization("AdminOnly");

        userGroup.MapGet("/{userId}",
                async Task<Results<Ok<User>, NotFound>> (Guid userId, IUserManagementService userService) =>
                    await userService.GetUserByIdAsync(userId) is { } user
                        ? TypedResults.Ok(user)
                        : TypedResults.NotFound())
            .RequireAuthorization();

        userGroup.MapPost("/",
            async Task<Results<Created<User>, BadRequest>> (LoginRegisterModel registration,
                IUserManagementService userService) =>
            {
                var user = new User(string.Empty, string.Empty, registration.Email, string.Empty, string.Empty,
                    string.Empty, string.Empty);

                var userAuthentication = new UserAuthentication(user.UserId, registration.Password);

                var userRegistration = new UserRegistration(user, userAuthentication);

                return await userService.CreateUserAsync(userRegistration) > 0
                    ? TypedResults.Created($"/api/users/{userRegistration.User.UserId}", user)
                    : TypedResults.BadRequest();
            }).WithValidation<UserRegistration>();

        userGroup.MapPut("/{userId:guid}",
                async Task<Results<NoContent, NotFound>> (Guid userId, UserUpdate user,
                    IUserManagementService userService) => await userService.UpdateUserAsync(userId, user) > 0
                    ? TypedResults.NoContent()
                    : TypedResults.NotFound())
            .WithValidation<UserUpdate>()
            .RequireAuthorization("UserOnly");

        userGroup.MapDelete("/{userId:guid}",
                async Task<Results<NoContent, NotFound>> (Guid userId, IUserManagementService userService) =>
                    await userService.DeleteUserAsync(userId) > 0
                        ? TypedResults.NoContent()
                        : TypedResults.NotFound())
            .RequireAuthorization("UserAndAdminOnly");

        userGroup.MapPost("/reset-password",
                async Task<Results<NoContent, NotFound>> (UserAuthentication userAuth,
                    IUserManagementService userService) => await userService.ResetPasswordAsync(userAuth) > 0
                    ? TypedResults.NoContent()
                    : TypedResults.NotFound())
            .WithValidation<UserAuthentication>()
            .RequireAuthorization("UserOnly");

        userGroup.MapGet("/{userId:guid}/applications", async Task<Results<Ok<IEnumerable<Application>>, BadRequest>> (ClaimsPrincipal user, IUserSpaceService spaceService) =>
        {
            var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (senderId is null)
                return TypedResults.BadRequest();
            
            var userId = Guid.Parse(senderId);
            
            var userApplications = await spaceService.GetApplicationsByUserIdAsync(userId);
            return TypedResults.Ok(userApplications);
        }).RequireAuthorization("UserOnly");

        userGroup.MapGet("/{userId:guid}/chats", async Task<Results<Ok<IEnumerable<Chat>>, BadRequest>> (ClaimsPrincipal user, IUserSpaceService spaceService) =>
            {
                var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
                if (senderId is null)
                    return TypedResults.BadRequest();
            
                var userId = Guid.Parse(senderId);
                
                var userChats = await spaceService.GetChatsForUserAsync(userId);
                return TypedResults.Ok(userChats);
            })
            .RequireAuthorization("UserOnly");

        return app;
    }
}