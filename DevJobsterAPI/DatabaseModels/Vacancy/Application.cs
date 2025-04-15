namespace DevJobsterAPI.DatabaseModels.Vacancy;

public class Application
{
    private Application()
    {
    }

    public Application(Guid userId, Guid vacancyId)
    {
        UserId = userId;
        VacancyId = vacancyId;
    }

    public Application(User.User user, Vacancy vacancy)
    {
        UserId = user.UserId;
        VacancyId = vacancy.VacancyId;

        User = user;
        Vacancy = vacancy;
    }

    public int ApplicationId { get; init; }
    public Guid UserId { get; init; }
    public Guid VacancyId { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    // Dapper will fill these properties
    // so telling compiler they're not null
    public User.User? User { get; init; }
    public Vacancy? Vacancy { get; init; }
}