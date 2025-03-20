namespace DevJobsterAPI.Models.RequestModels.Security;

public class RegisteredAccountShortView(string firstName, string lastName, bool role, DateTime createdAt)
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public required bool Role { get; set; } = role; // recruiter = true; user = false;
    public DateTime CreatedAt { get; set; } = createdAt;
}