using System.ComponentModel.DataAnnotations.Schema;

namespace DevJobsterAPI.Models.RequestModels.Chat;

public class MessageView(string body, Models.Chat.Chat chat)
{
    public required string Body { get; set; } = body;
    public Guid ChatId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RecruiterId { get; set; }
    
    public required Models.Chat.Chat Chat { get; set; } = chat;
    public Models.User.User? User { get; set; }
    public Models.Recruiter.Recruiter? Recruiter { get; set; }

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
}