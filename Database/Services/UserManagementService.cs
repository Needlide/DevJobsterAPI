using Dapper;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.Models.Recruiter;
using DevJobsterAPI.Models.User;

namespace DevJobsterAPI.Database.Services;

public class UserManagementService(DbContext dbContext) : IUserManagementService
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM users";
        return await connection.QueryAsync<User>(sql);
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM users WHERE user_id = @userId";
        return await connection.QuerySingleAsync<User>(sql, new { userId });
    }

    public async Task<int> CreateUserAsync(User user)
    {
        using var connection = dbContext.Connection;
        const string sql =
            """
                INSERT INTO users (user_id, first_name, last_name, email, role, skills, years_of_experience, location, english_level, created_at)
                VALUES (@UserId, @FirstName, @LastName, @Email, @Role, @Skills, @YearsOfExperience, @Location, @EnglishLevel, @CreatedAt);
            """;

        if (user.UserId == Guid.Empty) user.UserId = Guid.NewGuid();

        user.CreatedAt = DateTime.UtcNow;
        return await connection.ExecuteAsync(sql, user);
    }

    public async Task<int> UpdateUserAsync(User user)
    {
        using var connection = dbContext.Connection;
        const string sql =
            """
                UPDATE users SET first_name = @FirstName,
                last_name = @LastName, role = @Role, skills = @Skills,
                years_of_experience = @YearsOfExperience,
                location = @Location, english_level = @EnglishLevel
                WHERE user_id = @UserId;
            """;
        return await connection.ExecuteAsync(sql, user);
    }

    public async Task<int> DeleteUserAsync(Guid userId)
    {
        using var connection = dbContext.Connection;
        const string sql = "DELETE FROM users WHERE user_id = @UserId;";
        return await connection.ExecuteAsync(sql, new { userId });
    }

    public async Task<IEnumerable<Recruiter>> GetAllRecruitersAsync()
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM recruiters";
        return await connection.QueryAsync<Recruiter>(sql);
    }

    public async Task<Recruiter> GetRecruiterByIdAsync(Guid recruiterId)
    {
        using var connection = dbContext.Connection;
        const string sql = "SELECT * FROM recruiters WHERE recruiter_id = @RecruiterId;";
        return await connection.QuerySingleAsync<Recruiter>(sql, new { recruiterId });
    }

    public async Task<int> CreateRecruiterAsync(Recruiter recruiter)
    {
        using var connection = dbContext.Connection;
        const string sql =
            """
            INSERT INTO recruiters (recruiter_id, first_name, last_name, email, company, phone_number, notes, created_at)
            VALUES (@RecruiterId, @FirstName, @LastName, @Email, @Company, @PhoneNumber, @Notes, @CreatedAt);
            """;

        if (recruiter.RecruiterId == Guid.Empty) recruiter.RecruiterId = Guid.NewGuid();

        recruiter.CreatedAt = DateTime.UtcNow;
        return await connection.ExecuteAsync(sql, recruiter);
    }

    public async Task<int> UpdateRecruiterAsync(Recruiter recruiter)
    {
        using var connection = dbContext.Connection;
        const string sql =
            """
            UPDATE recruiters SET first_name = @FirstName,
            last_name = @LastName, phone_number = @PhoneNumber,
            notes = @Notes WHERE recruiter_id = @RecruiterId;
            """;
        return await connection.ExecuteAsync(sql, recruiter);
    }

    public async Task<int> DeleteRecruiterAsync(Guid recruiterId)
    {
        using var connection = dbContext.Connection;
        const string sql = "DELETE FROM recruiters WHERE recruiter_id = @RecruiterId;";
        return await connection.ExecuteAsync(sql, new { recruiterId });
    }

    public async Task<int> ResetPasswordAsync(Guid userId, string newPassword)
    {
        using var connection = dbContext.Connection;
        const string sql = "UPDATE user_authentication SET password_hash = @NewPassword WHERE user_id = @UserId;";
        return await connection.ExecuteAsync(sql, new { userId, newPassword });
    }
}