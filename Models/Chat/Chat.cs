namespace DevJobsterAPI.Models.Chat;

public class Chat
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public Guid RecruiterId { get; set; }
    public short NumberOfMessages { get; set; }
    public DateTime CreatedAt { get; set; }

    public required User.User User { get; set; }
    public required Recruiter.Recruiter Recruiter { get; set; }
    public List<Message> Messages { get; set; } = [];

    public Chat() {}

    public Chat(User.User user, Recruiter.Recruiter recruiter)
    {
        User = user;
        Recruiter = recruiter;
    }
}