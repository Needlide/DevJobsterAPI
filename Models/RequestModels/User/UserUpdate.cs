namespace DevJobsterAPI.Models.RequestModels.User;

public class UserUpdate(
    string firstName,
    string lastName,
    string role,
    string location,
    string yearsOfExperience = "0",
    string englishLevel = "1")
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public required string Role { get; set; } = role;
    public string? Skills { get; set; }
    public required string YearsOfExperience { get; set; } = yearsOfExperience;
    public required string Location { get; set; } = location;
    public required string EnglishLevel { get; set; } = englishLevel;
}