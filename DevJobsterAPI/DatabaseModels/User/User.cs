using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.Security;
using DevJobsterAPI.DatabaseModels.Vacancy;

namespace DevJobsterAPI.DatabaseModels.User;

public class User
{
    private User()
    {
    }

    public User(string firstName, string lastName, string email,
        string role, string yearsOfExperience, string location,
        string englishLevel, string? skills = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
        Skills = skills;
        YearsOfExperience = yearsOfExperience;
        Location = location;
        EnglishLevel = englishLevel;

        CreatedAt = DateTime.UtcNow;
    }

    public Guid UserId { get; init; } = Guid.NewGuid();

    // Dapper will fill these properties
    // so telling compiler they're not null
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Role { get; init; } = null!;
    public string? Skills { get; init; }
    public string YearsOfExperience { get; init; } = null!;
    public string Location { get; init; } = null!;
    public string EnglishLevel { get; init; } = null!;
    public DateTime CreatedAt { get; init; }

    public List<Message> Messages { get; init; } = [];
    public List<Chat.Chat> Chats { get; init; } = [];
    public List<Report> Reports { get; init; } = [];
    public List<Application> Applications { get; init; } = [];
}