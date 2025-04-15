namespace DevJobsterAPI.DatabaseModels.Vacancy;

public class Vacancy
{
    private Vacancy()
    {
    }

    public Vacancy(string title, string description, int salary, string requirements,
        string companyWebsite, string typeOfJob, string location, string country,
        Guid recruiterId, string? benefits = null)
    {
        Title = title;
        Description = description;
        Salary = salary;
        Requirements = requirements;
        CompanyWebsite = companyWebsite;
        TypeOfJob = typeOfJob;
        Location = location;
        Country = country;
        Benefits = benefits;

        CreatedAt = DateTime.UtcNow;
        RecruiterId = recruiterId;
    }

    public Vacancy(string title, string description, int salary, string requirements,
        string companyWebsite, string typeOfJob, string location, string country,
        Recruiter.Recruiter recruiter, string? benefits = null)
    {
        Title = title;
        Description = description;
        Salary = salary;
        Requirements = requirements;
        CompanyWebsite = companyWebsite;
        TypeOfJob = typeOfJob;
        Location = location;
        Country = country;
        Benefits = benefits;
        Recruiter = recruiter;

        CreatedAt = DateTime.UtcNow;
        RecruiterId = recruiter.RecruiterId;
    }

    public Guid VacancyId { get; init; } = Guid.NewGuid();
    public Guid RecruiterId { get; init; }

    // Dapper will fill these properties
    // so telling compiler they're not null
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public int Salary { get; init; }
    public string Requirements { get; init; } = null!;
    public string CompanyWebsite { get; init; } = null!;
    public string TypeOfJob { get; init; } = null!;
    public string Location { get; init; } = null!;
    public string Country { get; init; } = null!;
    public string? Benefits { get; init; }
    public DateTime CreatedAt { get; init; }

    public Recruiter.Recruiter? Recruiter { get; init; }
    public List<Application> Applications { get; init; } = [];
}