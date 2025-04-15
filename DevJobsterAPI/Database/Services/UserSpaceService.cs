using Dapper;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Chat;
using DevJobsterAPI.DatabaseModels.RequestModels.Security.Report;
using DevJobsterAPI.DatabaseModels.RequestModels.Vacancy;
using DevJobsterAPI.DatabaseModels.Vacancy;
using Npgsql;

namespace DevJobsterAPI.Database.Services;

public class UserSpaceService(IDbContext dbContext) : IUserSpaceService
{
    public async Task<IEnumerable<Vacancy>> GetAllVacanciesAsync()
    {
        try
        {
            const string sql = "SELECT * FROM vacancies";
            return await dbContext.Connection.QueryAsync<Vacancy>(sql);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<Vacancy?> GetVacancyByIdAsync(Guid vacancyId)
    {
        try
        {
            const string sql = "SELECT * FROM vacancies WHERE vacancy_id = @vacancyId";
            return await dbContext.Connection.QuerySingleOrDefaultAsync<Vacancy>(sql, new { vacancyId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> CreateVacancyAsync(AddVacancy vacancy)
    {
        try
        {
            const string sql = """
                               INSERT INTO vacancies
                               (vacancy_id, title, description,
                               salary, requirements, company_website,
                               type_of_job, location, country,
                               benefits, recruiter_id, created_at)
                               VALUES (@VacancyId, @Title, @Description,
                                @Salary, @Requirements, @CompanyWebsite,
                                @TypeOfJob, @Location, @Country,
                                @Benefits, @RecruiterId, @CreatedAt)
                               """;
            return await dbContext.Connection.ExecuteAsync(sql, vacancy);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> UpdateVacancyAsync(UpdateVacancy vacancy)
    {
        try
        {
            const string sql = """
                               UPDATE vacancies SET description = @Description,
                                                    salary = @Salary, requirements = @Requirements,
                                                    company_website = @CompanyWebsite,
                                                    type_of_job = @TypeOfJob,
                                                    location = @Location,
                                                    benefits = @Benefits
                                                    WHERE vacancy_id = @VacancyId;
                               """;
            return await dbContext.Connection.ExecuteAsync(sql, vacancy);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> DeleteVacancyAsync(Guid vacancyId)
    {
        try
        {
            const string sql = "DELETE FROM vacancies WHERE vacancy_id = @VacancyId;";
            return await dbContext.Connection.ExecuteAsync(sql, new { vacancyId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> CreateApplicationAsync(Guid userId, AddApplication addApplication)
    {
        try
        {
            const string sql = """
                               INSERT INTO applications
                               (user_id, vacancy_id, created_at)
                               VALUES (@UserId, @VacancyId, @CreatedAt)
                               RETURNING application_id;
                               """;
            return await dbContext.Connection.ExecuteScalarAsync<int>(sql,
                new { UserId = userId, addApplication.VacancyId, CreatedAt = DateTime.UtcNow });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(Guid userId)
    {
        try
        {
            const string sql = "SELECT * from applications where user_id = @UserId;";
            return await dbContext.Connection.QueryAsync<Application>(sql, new { UserId = userId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<Application>> GetApplicationsByVacancyIdAsync(Guid vacancyId)
    {
        try
        {
            const string sql = "SELECT * FROM applications where vacancy_id = @VacancyId;";
            return await dbContext.Connection.QueryAsync<Application>(sql, new { VacancyId = vacancyId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<Chat>> GetChatsForUserAsync(Guid userId)
    {
        try
        {
            const string sql = "SELECT * FROM chats where user_id = @UserId;";
            return await dbContext.Connection.QueryAsync<Chat>(sql, new { UserId = userId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<Chat?> GetChatByIdAsync(Guid chatId)
    {
        try
        {
            const string sql = "SELECT * FROM chats WHERE chat_id = @ChatId;";
            return await dbContext.Connection.QueryFirstOrDefaultAsync<Chat>(sql, new { ChatId = chatId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<Guid> CreateChatAsync(AddChat addChat)
    {
        try
        {
            var chat = new Chat(addChat.UserId, addChat.RecruiterId);

            const string sql = """
                               INSERT INTO chats
                                   (chat_id, user_id, recruiter_id,
                                    number_of_messages, created_at)
                                    VALUES (@ChatId, @UserId, @RecruiterId, @NumberOfMessages, @CreatedAt)
                                    RETURNING chat_id
                               """;

            var chatId = await dbContext.Connection.QuerySingleAsync<Guid>(
                sql,
                new
                {
                    chat.ChatId,
                    chat.UserId,
                    chat.RecruiterId,
                    chat.NumberOfMessages,
                    chat.CreatedAt
                });

            return chatId;
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> CreateMessageAsync(AddMessageWithSender addMessage)
    {
        try
        {
            if (addMessage.SenderRole == "Recruiter")
            {
                const string sqlRecruiter = """
                                                INSERT INTO messages
                                                (message_id, body, chat_id, recruiter_id, created_at)
                                                VALUES (@MessageId, @Body, @ChatId, @RecruiterId, @CreatedAt)
                                            """;

                var parametersRecruiter = new
                {
                    addMessage.MessageId,
                    addMessage.Body,
                    addMessage.ChatId,
                    RecruiterId = addMessage.SenderId,
                    addMessage.CreatedAt
                };

                return await dbContext.Connection.ExecuteAsync(sqlRecruiter, parametersRecruiter);
            }
            else
            {
                const string sqlUser = """
                                           INSERT INTO messages
                                           (message_id, body, chat_id, user_id, created_at)
                                           VALUES (@MessageId, @Body, @ChatId, @UserId, @CreatedAt)
                                       """;

                var parametersUser = new
                {
                    addMessage.MessageId,
                    addMessage.Body,
                    addMessage.ChatId,
                    UserId = addMessage.SenderId,
                    addMessage.CreatedAt
                };

                return await dbContext.Connection.ExecuteAsync(sqlUser, parametersUser);
            }
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<Message>> GetMessagesForChatAsync(Guid chatId)
    {
        try
        {
            const string sql = "SELECT * FROM messages WHERE chat_id = @ChatId;";
            return await dbContext.Connection.QueryAsync<Message>(sql, new { ChatId = chatId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<Message?> GetMessageByIdAsync(Guid messageId)
    {
        try
        {
            const string sql = "SELECT * FROM messages WHERE message_id = @MessageId;";
            return await dbContext.Connection.QueryFirstOrDefaultAsync<Message>(sql, new { MessageId = messageId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<bool> IsUserPartOfChatAsync(Guid chatId, Guid userId, string role)
    {
        const string sql = """
                           SELECT COUNT(*) FROM chats
                           WHERE chat_id = @ChatId
                             AND ((@Role = 'User' AND user_id = @UserId)
                               OR (@Role = 'Recruiter' AND recruiter_id = @UserId))
                           """;

        var count = await dbContext.Connection.ExecuteScalarAsync<int>(sql, new
        {
            ChatId = chatId,
            UserId = userId,
            Role = role
        });

        return count > 0;
    }

    public async Task<int> CreateReportAsync(AddReport report)
    {
        try
        {
            if (report.SenderType == UserType.User)
            {
                const string sql = """
                                   INSERT INTO reports (user_id, title, body, created_at, report_object_id)
                                   VALUES (@SenderId, @Title, @Body, @CreatedAt, @ReportObjectId)
                                   """;
                return await dbContext.Connection.ExecuteAsync(sql, report);
            }
            else
            {
                const string sql = """
                                   INSERT INTO reports (recruiter_id, title, body, created_at, report_object_id)
                                   VALUES (@SenderId, @Title, @Body, @CreatedAt, @ReportObjectId)
                                   """;
                return await dbContext.Connection.ExecuteAsync(sql, report);
            }
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }
}