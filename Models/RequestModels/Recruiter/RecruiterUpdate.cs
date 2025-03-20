namespace DevJobsterAPI.Models.RequestModels.Recruiter;

public class RecruiterUpdate(string firstName, string lastName, string phoneNumber, string? notes)
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public required string PhoneNumber { get; set; } = phoneNumber;
    public string? Notes { get; set; } = notes;
}