using DevJobsterAPI.Models.RequestModels.Recruiter;
using DevJobsterAPI.Models.RequestModels.User;

namespace DevJobsterAPI.Models.RequestModels.Admin;

public class AdminRegisteredAccountView(string firstName, string lastName)
{
    public required string FirstName { get; init; } = firstName;
    public required string LastName { get; init; } = lastName;
    public bool Checked { get; init; } = false;
    public UserProfileView? UserProfile { get; init; }
    public RecruiterView? Recruiter { get; init; }
}