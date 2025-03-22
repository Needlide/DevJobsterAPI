namespace DevJobsterAPI.Models.RequestModels.Vacancy;

public class AddVacancy(
    string title,
    string description,
    int salary,
    string requirements,
    string companyWebsite,
    string typeOfJob,
    string location,
    string country,
    string? benefits,
    DateTime createdAt)
{
    public required string Title { get; set; } = title;
    public required string Description { get; set; } = description;
    public int Salary { get; set; } = salary;
    public required string Requirements { get; set; } = requirements;
    public required string CompanyWebsite { get; set; } = companyWebsite;
    public required string TypeOfJob { get; set; } = typeOfJob;
    public required string Location { get; set; } = location;
    public required string Country { get; set; } = country;
    public string? Benefits { get; set; } = benefits;
    public DateTime CreatedAt { get; set; } = createdAt;
}