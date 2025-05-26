using System.Text.Json.Serialization;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;

[method: JsonConstructor]
public record UpdateVacancy(
    string Description,
    string Requirements,
    string CompanyWebsite,
    string TypeOfJob,
    string Location,
    Guid VacancyId,
    string? Benefits = null)
{
    public UpdateVacancy(DatabaseModels.Vacancy.Vacancy vacancy)
        : this(
            vacancy.Description,
            vacancy.Requirements,
            vacancy.CompanyWebsite,
            vacancy.TypeOfJob,
            vacancy.Location,
            vacancy.VacancyId,
            vacancy.Benefits)
    {
    }
}