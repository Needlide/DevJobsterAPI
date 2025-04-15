namespace DevJobsterAPI.DatabaseModels.Security;

public class UserAuthentication(Guid userId, string password)
{
    public Guid AuthId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userId;
    public string Password { get; set; } = password;
}