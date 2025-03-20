using DevJobsterAPI.Models.RequestModels.Recruiter;
using DevJobsterAPI.Models.RequestModels.User;

namespace DevJobsterAPI.Models.RequestModels.Admin;

public class AdminRegisteredAccountView(string firstName, string lastName)
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public bool Checked { get; set; } = false;
    public UserProfileView? UserProfile { get; set; }
    public RecruiterView? Recruiter { get; set; }
}