namespace DevJobsterAPI.Models.RequestModels.Security;

public class LoginRegisterModel(string email, string password)
{
    public required string Email { get; init; } = email;
    public required string Password { get; init; } = password;
}