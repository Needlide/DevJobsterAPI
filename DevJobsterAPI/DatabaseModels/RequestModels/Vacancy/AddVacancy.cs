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
    public Guid RecruiterId { get; init; }
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
        Guid recruiterId,
        string? benefits = null)
    {
        Title = title;
        Description = description;
        Salary = salary;
        Requirements = requirements;
        CompanyWebsite = companyWebsite;
        TypeOfJob = typeOfJob;
        Location = location;
        Country = country;
        RecruiterId = recruiterId;
        Benefits = benefits;
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
            vacancy.RecruiterId,
            vacancy.Benefits)
    {
        VacancyId = vacancy.VacancyId;
        CreatedAt = vacancy.CreatedAt;
    }
}