namespace DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;

public record UpdateVacancy(
    string Description,
    int Salary,
    string Requirements,
    string CompanyWebsite,
    string TypeOfJob,
    string Location,
    Guid VacancyId,
    string? Benefits = null)
{
    public UpdateVacancy(DatabaseModels.Vacancy.Vacancy vacancy)
        : this
        (
            vacancy.Description,
            vacancy.Salary,
            vacancy.Requirements,
            vacancy.CompanyWebsite,
            vacancy.TypeOfJob,
            vacancy.Location,
            vacancy.VacancyId,
            vacancy.Benefits
        )
    {
    }
}