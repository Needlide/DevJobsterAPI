namespace DevJobsterAPI.DatabaseModels.Chat;

public class Chat
{
    private Chat()
    {
    }

    public Chat(Guid userId, Guid recruiterId)
    {
        UserId = userId;
        RecruiterId = recruiterId;
    }

    public Chat(User.User user, Recruiter.Recruiter recruiter)
    {
        UserId = user.UserId;
        RecruiterId = recruiter.RecruiterId;

        User = user;
        Recruiter = recruiter;
    }

    public Guid ChatId { get; init; } = Guid.NewGuid();
    public Guid UserId { get; init; }
    public Guid RecruiterId { get; init; }
    public short NumberOfMessages { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public User.User? User { get; init; }
    public Recruiter.Recruiter? Recruiter { get; init; }
    public List<Message> Messages { get; init; } = [];
}