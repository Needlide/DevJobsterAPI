namespace DevJobsterAPI.Models.RequestModels.Recruiter;

public class RecruiterView(string firstName, string lastName, string email, string company, string phoneNumber)
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public required string Company { get; set; } = company;
    public string? Notes { get; set; }
}