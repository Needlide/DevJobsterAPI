using DevJobsterAPI.Models.Chat;
using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Models.Recruiter;

public class Recruiter
{
    public Guid RecruiterId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Company { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Message> Messages { get; set; } = [];
    public List<Chat.Chat> Chats { get; set; } = [];
    public List<Vacancy.Vacancy> Vacancies { get; set; } = [];
    public List<Report> Reports { get; set; } = [];
    public List<RegisteredAccount> RegisteredAccounts { get; set; } = [];

    public Recruiter() {}

    public Recruiter(string firstName, string lastName, string email, string company, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Company = company;
        PhoneNumber = phoneNumber;
    }
}