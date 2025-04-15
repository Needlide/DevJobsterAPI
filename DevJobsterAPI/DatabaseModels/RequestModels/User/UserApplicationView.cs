namespace DevJobsterAPI.DatabaseModels.RequestModels.User;

public record UserApplicationView(
    string FirstName,
    string LastName,
    string Role,
    string Location,
    string YearsOfExperience,
    string EnglishLevel);