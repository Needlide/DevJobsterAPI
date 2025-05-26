using System.Text.Json.Serialization;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;

public record AddVacancy
{
    public string Title { get; init; }
    public string Description { get; init; }
    public int Salary { get; init; }
    public string Requirements { get; init; }
    public string CompanyWebsite { get; init; }
    public string TypeOfJob { get; init; }
    public string Location { get; init; }
    public string Country { get; init; }
    public string? Benefits { get; init; }

    public Guid VacancyId { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [JsonConstructor]
    public AddVacancy(
        string title,
        string description,
        int salary,
        string requirements,
        string companyWebsite,
        string typeOfJob,
        string location,
        string country,
        string? benefits = null,
        Guid vacancyId = default,
        DateTime createdAt = default)
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
        VacancyId = vacancyId == default ? Guid.NewGuid() : vacancyId;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
    }

    // Optional constructor for internal mapping
    public AddVacancy(DatabaseModels.Vacancy.Vacancy vacancy)
        : this(
            vacancy.Title,
            vacancy.Description,
            vacancy.Salary,
            vacancy.Requirements,
            vacancy.CompanyWebsite,
            vacancy.TypeOfJob,
            vacancy.Location,
            vacancy.Country,
            vacancy.Benefits)
    {
        VacancyId = vacancy.VacancyId;
        CreatedAt = vacancy.CreatedAt;
    }
}