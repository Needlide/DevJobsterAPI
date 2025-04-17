namespace DevJobsterAPI.DatabaseModels.RequestModels.User;

public record UserProfileView(
    string FirstName,
    string LastName,
    string Role,
    string? Skills,
    string Location,
    string YearsOfExperience,
    string EnglishLevel);