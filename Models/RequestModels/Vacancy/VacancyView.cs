namespace DevJobsterAPI.Models.RequestModels.Vacancy;

public class VacancyView(
    string title,
    string description,
    string requirements,
    string companyWebsite,
    string typeOfJob,
    string location,
    string country,
    Models.Recruiter.Recruiter recruiter)
{
    public required string Title { get; set; } = title;
    public required string Description { get; set; } = description;
    public int Salary { get; set; }
    public required string Requirements { get; set; } = requirements;
    public required string CompanyWebsite { get; set; } = companyWebsite;
    public required string TypeOfJob { get; set; } = typeOfJob;
    public required string Location { get; set; } = location;
    public required string Country { get; set; } = country;
    public string? Benefits { get; set; }
    public DateTime CreatedAt { get; set; }

    public required Models.Recruiter.Recruiter Recruiter { get; set; } = recruiter;
}