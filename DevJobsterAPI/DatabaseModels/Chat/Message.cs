using System.ComponentModel.DataAnnotations.Schema;
using DevJobsterAPI.Common;

namespace DevJobsterAPI.DatabaseModels.Chat;

public class Message
{
    private Message()
    {
    }

    public Message(string body, Guid chatId, Guid? recruiterId = null, Guid? userId = null)
    {
        ChatId = chatId;
        Body = body;
        RecruiterId = recruiterId;
        UserId = userId;
    }

    public Message(string body, Chat chat, Guid? recruiterId = null, Guid? userId = null)
    {
        ChatId = chat.ChatId;
        Chat = chat;
        Body = body;
        RecruiterId = recruiterId;
        UserId = userId;
    }

    public Guid MessageId { get; init; } = Guid.NewGuid();

    // Dapper will fill these properties
    // so telling compiler they're not null
    public string Body { get; init; } = null!;
    public Guid ChatId { get; init; }
    public Guid? UserId { get; init; }
    public Guid? RecruiterId { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public Chat? Chat { get; init; }
    public User.User? User { get; init; }
    public Recruiter.Recruiter? Recruiter { get; init; }

    [NotMapped] public Guid? SenderId => UserId ?? RecruiterId;

    [NotMapped]
    public UserType? SenderType
    {
        get
        {
            if (UserId.HasValue) return UserType.User;
            return RecruiterId.HasValue ? UserType.Recruiter : null;
        }
    }
}