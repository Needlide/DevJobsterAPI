using DevJobsterAPI.Models.Chat;
using DevJobsterAPI.Models.Security;
using DevJobsterAPI.Models.Vacancy;

namespace DevJobsterAPI.Models.User;

public class User
{
    public User()
    {
    }

    public User(string firstName, string lastName, string email, string role, string yearsOfExperience, string location,
        string englishLevel)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
        YearsOfExperience = yearsOfExperience;
        Location = location;
        EnglishLevel = englishLevel;
    }

    public Guid UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public string? Skills { get; set; }
    public required string YearsOfExperience { get; set; } = "0";
    public required string Location { get; set; }
    public required string EnglishLevel { get; set; } = "1";
    public DateTime CreatedAt { get; set; }

    public List<Message> Messages { get; set; } = [];
    public List<Chat.Chat> Chats { get; set; } = [];
    public List<Report> Reports { get; set; } = [];
    public List<Application> Applications { get; set; } = [];
    public List<RegisteredAccount> RegisteredAccounts { get; set; } = [];
}