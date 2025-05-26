namespace DevJobsterAPI.DatabaseModels.RequestModels.Chat;

public record ChatView(Guid ChatId, Guid UserId, Guid RecruiterId, short NumberOfMessages);