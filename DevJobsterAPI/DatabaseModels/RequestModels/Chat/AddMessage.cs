namespace DevJobsterAPI.DatabaseModels.RequestModels.Chat;

public record AddMessage(Guid ChatId, string Body);