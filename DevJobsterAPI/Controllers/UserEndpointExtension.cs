using System.Security.Claims;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.RequestModels.User;
using DevJobsterAPI.DatabaseModels.Security;
using DevJobsterAPI.DatabaseModels.User;
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
            
            var userViews = users.Select(u => new UserProfileView(u.FirstName, u.LastName, u.Role, u.Skills, u.Location, u.YearsOfExperience, u.EnglishLevel));
            
            return TypedResults.Ok(userViews);
        }).RequireAuthorization("AdminOnly");

        userGroup.MapGet("/{userId}",
                async Task<Results<Ok<UserProfileView>, NotFound>> (Guid userId, IUserManagementService userService) =>
                {
                    var user = await userService.GetUserByIdAsync(userId);
                    
                    if (user == null)
                        return TypedResults.NotFound();

                    var userView = new UserProfileView(user.FirstName, user.LastName, user.Role, user.Skills, user.Location, user.YearsOfExperience, user.EnglishLevel);
                    return TypedResults.Ok(userView);
                })
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

        userGroup.MapGet("/{userId:guid}/applications",
            async Task<Results<Ok<List<UserApplicationView>>, BadRequest>> (
                ClaimsPrincipal user,
                IUserSpaceService spaceService,
                IUserManagementService userService) =>
            {
                var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (senderId is null)
                    return TypedResults.BadRequest();
                
                var userApplications = await spaceService.GetApplicationsByUserIdAsync(Guid.Parse(senderId));

                var userApplicationViews = new List<UserApplicationView>();

                foreach (var ua in userApplications)
                {
                    var applicationUser = ua.User ?? await userService.GetUserByIdAsync(ua.UserId);

                    if (applicationUser is null)
                    {
                        continue;
                    }

                    userApplicationViews.Add(new UserApplicationView(
                        applicationUser.FirstName,
                        applicationUser.LastName,
                        applicationUser.Role,
                        applicationUser.Location,
                        applicationUser.YearsOfExperience,
                        applicationUser.EnglishLevel));
                }

                return TypedResults.Ok(userApplicationViews);
            }).RequireAuthorization("UserOnly");

        userGroup.MapGet("/{userId:guid}/chats",
                async Task<Results<Ok<IEnumerable<Chat>>, BadRequest>> (ClaimsPrincipal user,
                    IUserSpaceService spaceService) =>
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