namespace DevJobsterAPI.Models.Vacancy;

public class Vacancy
{
    public Vacancy()
    {
    }

    public Vacancy(string title, string description, string requirements, string companyWebsite, string typeOfJob,
        string location, string country, Recruiter.Recruiter recruiter)
    {
        Title = title;
        Description = description;
        Requirements = requirements;
        CompanyWebsite = companyWebsite;
        TypeOfJob = typeOfJob;
        Location = location;
        Country = country;
        Recruiter = recruiter;
    }

    public Guid VacancyId { get; set; }
    public Guid RecruiterId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Salary { get; set; }
    public required string Requirements { get; set; }
    public required string CompanyWebsite { get; set; }
    public required string TypeOfJob { get; set; }
    public required string Location { get; set; }
    public required string Country { get; set; }
    public string? Benefits { get; set; }
    public DateTime CreatedAt { get; set; }

    public required Recruiter.Recruiter Recruiter { get; set; }
    public List<Application> Applications { get; set; } = [];
}