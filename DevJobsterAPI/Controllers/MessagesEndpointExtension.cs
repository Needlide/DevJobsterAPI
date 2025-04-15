using System.Security.Claims;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class MessagesEndpointExtension
{
    public static WebApplication MapMessagesEndpoint(this WebApplication app)
    {
        var messageGroup = app.MapGroup("/api/messages");

        messageGroup.MapGet("/{messageId:guid}",
                async Task<Results<Ok<Message>, NotFound>> (Guid messageId, IUserSpaceService userSpaceService) =>
                    await userSpaceService.GetMessageByIdAsync(messageId) is { } message
                        ? TypedResults.Ok(message)
                        : TypedResults.NotFound())
            .RequireAuthorization();

        messageGroup.MapPost("/",
                async Task<Results<Created<AddMessageWithSender>, BadRequest, ForbidHttpResult>> (
                    AddMessage addMessage,
                    ClaimsPrincipal user,
                    IUserSpaceService userSpaceService) =>
                {
                    var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var role = user.FindFirst(ClaimTypes.Role)?.Value;

                    if (senderId is null || role is null)
                        return TypedResults.BadRequest();

                    var senderGuid = Guid.Parse(senderId);

                    var isPartOfChat = await userSpaceService.IsUserPartOfChatAsync(
                        addMessage.ChatId,
                        senderGuid,
                        role
                    );

                    if (!isPartOfChat)
                        return TypedResults.Forbid();
                    
                    var message = new AddMessageWithSender(
                        MessageId: Guid.NewGuid(),
                        ChatId: addMessage.ChatId,
                        Body: addMessage.Body,
                        SenderId: Guid.Parse(senderId),
                        SenderRole: role,
                        CreatedAt: DateTime.UtcNow
                    );

                    var result = await userSpaceService.CreateMessageAsync(message);

                    return result > 0
                        ? TypedResults.Created($"/api/messages/{message.MessageId}", message)
                        : TypedResults.BadRequest();
                })
            .WithValidation<MessageValidator>()
            .RequireAuthorization("UserAndRecruiterOnly");

        return app;
    }
}