using System.Security.Claims;
using DevJobsterAPI.ApiModels;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat;
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

        var userViews = users.Select(u => new UserProfileView(u.FirstName, u.LastName, u.Role, u.Skills, u.Location,
            u.YearsOfExperience, u.EnglishLevel));

        return TypedResults.Ok(ApiResponseFactory.Success(userViews));
    }).RequireAuthorization("AdminOnly");

    userGroup.MapGet("/me",
            async Task<Results<Ok<ApiResponse<UserProfileView>>, NotFound<ApiResponse<UserProfileView>>, BadRequest<ApiResponse<UserProfileView>>>> (
                ClaimsPrincipal claimsPrincipal, 
                IUserManagementService userService) =>
            {
                var senderId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<UserProfileView>(
                        "User ID not found in claims", "INVALID_USER"));

                var userId = Guid.Parse(senderId);
                
                var user = await userService.GetUserByIdAsync(userId);

                if (user == null)
                    return TypedResults.NotFound(ApiResponseFactory.Fail<UserProfileView>(
                        "User not found", "NOT_FOUND"));

                var userView = new UserProfileView(user.FirstName, user.LastName, user.Role, user.Skills,
                    user.Location, user.YearsOfExperience, user.EnglishLevel);
                    
                return TypedResults.Ok(ApiResponseFactory.Success(userView));
            })
        .RequireAuthorization();

    userGroup.MapPost("/",
        async Task<Results<Created<ApiResponse<User>>, BadRequest<ApiResponse<User>>>> (
            LoginRegisterModel registration,
            IUserManagementService userService) =>
        {
            var user = new User(string.Empty, string.Empty, registration.Email, string.Empty, string.Empty,
                string.Empty, string.Empty);

            var userAuthentication = new UserAuthentication(user.UserId, registration.Password);

            var userRegistration = new UserRegistration(user, userAuthentication);

            var result = await userService.CreateUserAsync(userRegistration);
            
            if (result > 0)
                return TypedResults.Created(
                    $"/api/users/{userRegistration.User.UserId}", 
                    ApiResponseFactory.Success(user, "User created successfully"));
            else
                return TypedResults.BadRequest(ApiResponseFactory.Fail<User>(
                    "Failed to create user", "CREATION_FAILED"));
        }).WithValidation<UserRegistration>().AllowAnonymous();

    userGroup.MapPut("/me",
            async Task<Results<Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>, BadRequest<ApiResponse<object>>>> (
                ClaimsPrincipal claimsPrincipal, 
                UserUpdate user,
                IUserManagementService userService) =>
            {
                var senderId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<object>(
                        "User ID not found in claims", "INVALID_USER"));

                var userId = Guid.Parse(senderId);
                
                var result = await userService.UpdateUserAsync(userId, user);
                
                if (result > 0)
                    return TypedResults.Ok(ApiResponseFactory.Success<object>(null, "User updated successfully"));
                else
                    return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                        "User not found or update failed", "UPDATE_FAILED"));
            })
        .WithValidation<UserUpdate>()
        .RequireAuthorization("UserOnly");

    userGroup.MapDelete("/me",
            async Task<Results<Ok<ApiResponse<object>>, NotFound<ApiResponse<object>>, BadRequest<ApiResponse<object>>>> (
                ClaimsPrincipal claimsPrincipal, 
                IUserManagementService userService) =>
            {
                var senderId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<object>(
                        "User ID not found in claims", "INVALID_USER"));

                var userId = Guid.Parse(senderId);
                
                var result = await userService.DeleteUserAsync(userId);
                
                if (result > 0)
                    return TypedResults.Ok(ApiResponseFactory.Success<object>(null, "User deleted successfully"));
                else
                    return TypedResults.NotFound(ApiResponseFactory.Fail<object>(
                        "User not found", "NOT_FOUND"));
            })
        .RequireAuthorization("UserAndAdminOnly");

    userGroup.MapPost("/reset-password",
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
        .RequireAuthorization("UserOnly");

    userGroup.MapGet("/my-applications",
        async Task<Results<Ok<ApiResponse<List<UserApplicationView>>>, BadRequest<ApiResponse<List<UserApplicationView>>>>> (
            ClaimsPrincipal user,
            IUserSpaceService spaceService,
            IUserManagementService userService) =>
        {
            var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (senderId is null)
                return TypedResults.BadRequest(ApiResponseFactory.Fail<List<UserApplicationView>>(
                    "User ID not found in claims", "INVALID_USER"));

            var userApplications = await spaceService.GetApplicationsByUserIdAsync(Guid.Parse(senderId));

            var userApplicationViews = new List<UserApplicationView>();

            foreach (var ua in userApplications)
            {
                var applicationUser = ua.User ?? await userService.GetUserByIdAsync(ua.UserId);

                if (applicationUser is null) continue;

                userApplicationViews.Add(new UserApplicationView(
                    applicationUser.FirstName,
                    applicationUser.LastName,
                    applicationUser.Role,
                    applicationUser.Location,
                    applicationUser.YearsOfExperience,
                    applicationUser.EnglishLevel));
            }

            return TypedResults.Ok(ApiResponseFactory.Success(userApplicationViews));
        }).RequireAuthorization("UserOnly");

    userGroup.MapGet("/my-chats",
            async Task<Results<Ok<ApiResponse<List<ChatView>>>, BadRequest<ApiResponse<List<ChatView>>>>> (
                ClaimsPrincipal user,
                IUserSpaceService spaceService) =>
            {
                var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (senderId is null)
                    return TypedResults.BadRequest(ApiResponseFactory.Fail<List<ChatView>>(
                        "User ID not found in claims", "INVALID_USER"));

                var userId = Guid.Parse(senderId);

                var userChats = await spaceService.GetChatsForUserAsync(userId);
                
                List<ChatView> userChatViews = [];
                
                userChatViews.AddRange(userChats.Select(chat => 
                    new ChatView(chat.ChatId, chat.UserId, chat.RecruiterId, chat.NumberOfMessages)));

                return TypedResults.Ok(ApiResponseFactory.Success(userChatViews));
            })
        .RequireAuthorization("UserOnly");

    return app;
}
}