namespace DevJobsterAPI.Models.RequestModels.Recruiter;

public class RecruiterUpdate(string firstName, string lastName, string phoneNumber, string? notes)
{
    public required string FirstName { get; init; } = firstName;
    public required string LastName { get; init; } = lastName;
    public required string PhoneNumber { get; init; } = phoneNumber;
    public string? Notes { get; init; } = notes;
}