using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Security.Report;
using DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;
using DevJobsterAPI.DatabaseModels.Vacancy;

namespace DevJobsterAPI.Database.Abstract;

public interface IUserSpaceService
{
    Task<IEnumerable<Vacancy>> GetAllVacanciesAsync();
    Task<Vacancy?> GetVacancyByIdAsync(Guid vacancyId);
    Task<int> CreateVacancyAsync(AddVacancy vacancy);
    Task<int> UpdateVacancyAsync(UpdateVacancy vacancy);
    Task<int> DeleteVacancyAsync(Guid vacancyId);

    Task<int> CreateApplicationAsync(Guid userId, AddApplication addApplication);
    Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(Guid userId);
    Task<IEnumerable<Application>> GetApplicationsByVacancyIdAsync(Guid vacancyId);
    Task<IEnumerable<Vacancy>> GetVacanciesByUserIdAsync(Guid userId);

    Task<IEnumerable<Chat>> GetChatsForUserAsync(Guid userId);
    Task<Chat?> GetChatByIdAsync(Guid chatId);
    Task<Guid> CreateChatAsync(AddChat addChat);
    Task<int> CreateMessageAsync(AddMessageWithSender addMessage);
    Task<bool> IsUserPartOfChatAsync(Guid chatId, Guid userId, string role);

    Task<IEnumerable<Message>> GetMessagesForChatAsync(Guid chatId);
    Task<Message?> GetMessageByIdAsync(Guid messageId);

    Task<int> CreateReportAsync(AddReport report);
}