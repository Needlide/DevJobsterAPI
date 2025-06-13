using System.Security.Claims;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevJobsterAPI.Controllers;

public static class MessagesEndpointExtension
{
    public static WebApplication MapMessagesEndpoint(this WebApplication app)
    {
        var messageGroup = app.MapGroup("/api/messages").RequireCors("AllowFrontend");

        messageGroup.MapGet("/{messageId:guid}",
                async Task<Results<Ok<MessageView>, NotFound>> (Guid messageId, IUserSpaceService userSpaceService) =>
                {
                    var message = await userSpaceService.GetMessageByIdAsync(messageId);

                    if (message == null) return TypedResults.NotFound();

                    var messageView = new MessageView(
                        message.Body,
                        message.ChatId,
                        message.UserId,
                        message.RecruiterId);

                    return TypedResults.Ok(messageView);
                })
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
                        Guid.NewGuid(),
                        addMessage.ChatId,
                        addMessage.Body,
                        Guid.Parse(senderId),
                        role,
                        DateTime.UtcNow
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