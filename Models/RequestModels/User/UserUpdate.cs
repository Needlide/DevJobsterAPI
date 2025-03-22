namespace DevJobsterAPI.Models.RequestModels.User;

public class UserUpdate(
    string firstName,
    string lastName,
    string role,
    string location,
    string yearsOfExperience = "0",
    string englishLevel = "1")
{
    public required string FirstName { get; init; } = firstName;
    public required string LastName { get; init; } = lastName;
    public required string Role { get; init; } = role;
    public string? Skills { get; init; }
    public required string YearsOfExperience { get; init; } = yearsOfExperience;
    public required string Location { get; init; } = location;
    public required string EnglishLevel { get; init; } = englishLevel;
}