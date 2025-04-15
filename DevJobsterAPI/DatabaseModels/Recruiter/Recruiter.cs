using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.Recruiter;

public class Recruiter
{
    private Recruiter()
    {
    }

    public Recruiter(string firstName, string lastName, string email, string company, string phoneNumber,
        string? notes = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Company = company;
        PhoneNumber = phoneNumber;
        Notes = notes;

        CreatedAt = DateTime.UtcNow;
    }

    public Guid RecruiterId { get; init; } = Guid.NewGuid();

    // Dapper will fill these properties
    // so telling compiler they're not null
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Company { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }

    public List<Message> Messages { get; init; } = [];
    public List<Chat.Chat> Chats { get; init; } = [];
    public List<Vacancy.Vacancy> Vacancies { get; init; } = [];
    public List<Report> Reports { get; init; } = [];
    public List<RegisteredAccount> RegisteredAccounts { get; init; } = [];
}