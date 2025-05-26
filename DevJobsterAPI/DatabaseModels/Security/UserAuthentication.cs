namespace DevJobsterAPI.DatabaseModels.Security;

public class UserAuthentication(Guid userId, string password)
{
    public Guid AuthId { get; set; } = Guid.NewGuid();
    public string Password { get; set; } = password;
}