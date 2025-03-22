using DevJobsterAPI.Models.Chat;
using DevJobsterAPI.Models.Vacancy;

namespace DevJobsterAPI.Database.Abstract;

public interface IUserSpaceService
{
    Task<IEnumerable<Vacancy>> GetAllVacanciesAsync();
    Task<Vacancy> GetVacancyByIdAsync(Guid vacancyId);
    Task CreateVacancyAsync(Vacancy vacancy);
    Task UpdateVacancyAsync(Vacancy vacancy);
    Task DeleteVacancyAsync(Guid vacancyId);

    Task CreateApplicationAsync(Application application);
    Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(Guid userId);
    Task<IEnumerable<Application>> GetApplicationsByVacancyIdAsync(Guid vacancyId);

    Task<IEnumerable<Chat>> GetChatsForUserAsync(Guid userId);
    Task<Chat> GetChatByIdAsync(Guid chatId);
    Task<Chat> CreateChatAsync(Chat chat);
    Task AddMessageToChatAsync(Guid chatId, Message message);

    Task<IEnumerable<Message>> GetMessagesForChatAsync(Guid chatId);
    Task<Message> GetMessageByIdAsync(Guid messageId);
}