namespace DevJobsterAPI.DatabaseModels.RequestModels.User;

public record UserUpdate(
    string FirstName,
    string LastName,
    string Role,
    string? Skills,
    string Location,
    string YearsOfExperience = "0",
    string EnglishLevel = "1");