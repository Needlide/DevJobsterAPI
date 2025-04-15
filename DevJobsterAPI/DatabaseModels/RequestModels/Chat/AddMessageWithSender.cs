namespace DevJobsterAPI.DatabaseModels.RequestModels.Chat;

public record AddMessageWithSender(
    Guid MessageId,
    Guid ChatId,
    string Body,
    Guid SenderId,
    string SenderRole,
    DateTime CreatedAt
);