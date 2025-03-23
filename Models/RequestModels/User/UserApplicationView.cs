namespace DevJobsterAPI.Models.RequestModels.User;

public class UserApplicationView(
    string firstName,
    string lastName,
    string role,
    string location,
    string yearsOfExperience,
    string englishLevel)
{
    public required string FirstName { get; set; } = firstName;
    public required string LastName { get; set; } = lastName;
    public required string Role { get; set; } = role;
    public required string Location { get; set; } = location;
    public required string YearsOfExperience { get; set; } = yearsOfExperience;
    public required string EnglishLevel { get; set; } = englishLevel;
}