using System.ComponentModel.DataAnnotations.Schema;

namespace DevJobsterAPI.Models.Chat;

public class Message
{
    public Guid MessageId { get; set; }
    public required string Body { get; set; }
    public Guid ChatId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RecruiterId { get; set; }
    public DateTime CreatedAt { get; set; }

    public required Chat Chat { get; set; }
    public User.User? User { get; set; }
    public Recruiter.Recruiter? Recruiter { get; set; }

    [NotMapped]
    public Guid? SenderId => UserId ?? RecruiterId;

    [NotMapped]
    public string? SenderType
    {
        get
        {
            if (UserId.HasValue) return "User";
            return RecruiterId.HasValue ? "Recruiter" : null;
        }
    }
    
    public Message() {}

    public Message(string body, Chat chat)
    {
        Chat = chat;
        Body = body;
    }
}