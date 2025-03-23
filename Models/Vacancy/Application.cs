namespace DevJobsterAPI.Models.Vacancy;

public class Application
{
    public Application()
    {
    }

    public Application(User.User user, Vacancy vacancy)
    {
        User = user;
        Vacancy = vacancy;
    }

    public int ApplicationId { get; set; }
    public Guid UserId { get; set; }
    public Guid VacancyId { get; set; }
    public DateTime CreatedAt { get; set; }

    public required User.User User { get; set; }
    public required Vacancy Vacancy { get; set; }
}