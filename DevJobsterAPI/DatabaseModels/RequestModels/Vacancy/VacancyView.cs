namespace DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;

public record VacancyView(
    Guid VacancyId,
    string Title,
    string Description,
    string Requirements,
    string CompanyWebsite,
    string TypeOfJob,
    string Location,
    string Country,
    Recruiter.RecruiterView? Recruiter);