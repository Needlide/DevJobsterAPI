using DevJobsterAPI.Models.Vacancy;

namespace DevJobsterAPI.Models.RequestModels.User;

public class UserProfileView(
    string firstName,
    string lastName,
    string email,
    string role,
    string location,
    string yearsOfExperience,
    string englishLevel)
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public required string Email { get; set; } = email;
    public required string Role { get; set; } = role;
    public string? Skills { get; set; }
    public required string YearsOfExperience { get; set; } = yearsOfExperience;
    public required string Location { get; set; } = location;
    public required string EnglishLevel { get; set; } = englishLevel;

    public List<Application> Applications { get; set; } = [];
}