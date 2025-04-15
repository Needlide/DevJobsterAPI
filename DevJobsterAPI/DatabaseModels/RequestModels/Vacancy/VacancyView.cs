namespace DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;

public record VacancyView(
    string Title,
    string Description,
    string Requirements,
    string CompanyWebsite,
    string TypeOfJob,
    string Location,
    string Country,
    DatabaseModels.Recruiter.Recruiter Recruiter);