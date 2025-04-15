using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class ChatsEndpointExtension
{
    public static WebApplication MapChatsEndpoint(this WebApplication app)
    {
        var chatGroup = app.MapGroup("/api/chats");

        chatGroup.MapGet("/{chatId:guid}",
                async Task<Results<Ok<Chat>, NotFound>> (Guid chatId, IUserSpaceService userSpaceService) =>
                    await userSpaceService.GetChatByIdAsync(chatId) is { } chat
                        ? TypedResults.Ok(chat)
                        : TypedResults.NotFound())
            .RequireAuthorization();

        chatGroup.MapPost("/",
                async Task<Results<Created<Guid>, BadRequest>> (AddChat chat, IUserSpaceService userSpaceService) =>
                {
                    var chatId = await userSpaceService.CreateChatAsync(chat);

                    if (chatId == Guid.Empty)
                    {
                        return TypedResults.BadRequest();
                    }

                    return TypedResults.Created($"/api/chats/{chatId}", chatId);
                })
            .RequireAuthorization("UserAndRecruiterOnly");

        chatGroup.MapGet("/{chatId:guid}/messages", async (Guid chatId, IUserSpaceService spaceService) =>
        {
            var messages = await spaceService.GetMessagesForChatAsync(chatId);
            return TypedResults.Ok(messages);
        }).RequireAuthorization();

        return app;
    }
}