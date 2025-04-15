using Dapper;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels;
using DevJobsterAPI.DatabaseModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.RequestModels.User;
using DevJobsterAPI.DatabaseModels.Security;
using DevJobsterAPI.DatabaseModels.User;
using Npgsql;

namespace DevJobsterAPI.Database.Services;

public class UserManagementService(IDbContext dbContext) : IUserManagementService
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            const string sql = "SELECT * FROM users";
            return await dbContext.Connection.QueryAsync<User>(sql);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        try
        {
            const string sql = "SELECT * FROM users WHERE user_id = @userId";
            return await dbContext.Connection.QuerySingleOrDefaultAsync<User>(sql, new { userId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> CreateUserAsync(UserRegistration userRegistration)
    {
        try
        {
            using var transaction = dbContext.Connection.BeginTransaction();

            try
            {
                // Insert user into unified_users
                const string usersSql =
                    """
                    INSERT INTO unified_users (user_id, email, user_type, created_at)
                    VALUES (@UserId, @Email, @UserType, @CreatedAt); 
                    """;

                await dbContext.Connection.ExecuteAsync(usersSql, new
                {
                    userRegistration.User.UserId,
                    userRegistration.User.Email,
                    UserType = "user",
                    CreatedAt = DateTime.UtcNow
                }, transaction);

                // Insert user into users
                const string userSql =
                    """
                        INSERT INTO users (user_id, first_name,
                        last_name, email, role, skills, years_of_experience,
                        location, english_level, created_at)
                        VALUES (@UserId, @FirstName, @LastName, @Email, @Role, @Skills,
                        @YearsOfExperience, @Location, @EnglishLevel, @CreatedAt);
                    """;

                await dbContext.Connection.ExecuteAsync(userSql, userRegistration.User, transaction);

                // Insert user's credentials
                const string authSql =
                    """
                        INSERT INTO user_authentication (auth_id, user_id, password_hash)
                        VALUES (@AuthId, @UserId, @PasswordHash);
                    """;

                userRegistration.UserAuthentication.UserId = userRegistration.User.UserId;
                
                await dbContext.Connection.ExecuteAsync(
                    authSql,
                    new
                    {
                        userRegistration.UserAuthentication.AuthId,
                        userRegistration.UserAuthentication.UserId,
                        PasswordHash = PasswordService.HashPassword(userRegistration.UserAuthentication.Password)
                    }, 
                    transaction);

                // Insert user into registered accounts
                const string registeredSql = """
                                             INSERT INTO registered_accounts (user_id, created_at)
                                             VALUES (@UserId, @CreatedAt)
                                             """;
                await dbContext.Connection.ExecuteAsync(registeredSql,
                    new { userRegistration.User.UserId, CreatedAt = DateTime.UtcNow }, transaction);

                transaction.Commit();

                return 1;
            }
            catch
            {
                if (transaction.Connection != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> UpdateUserAsync(Guid userId, UserUpdate user)
    {
        try
        {
            const string sql =
                """
                    UPDATE users SET first_name = @FirstName,
                    last_name = @LastName, role = @Role, skills = @Skills,
                    years_of_experience = @YearsOfExperience,
                    location = @Location, english_level = @EnglishLevel
                    WHERE user_id = @UserId;
                """;
            return await dbContext.Connection.ExecuteAsync(
                sql, 
                new
            {
                UserId = userId,
                user.FirstName,
                user.LastName,
                user.Role,
                user.Skills,
                user.YearsOfExperience,
                user.Location,
                user.EnglishLevel
            });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> DeleteUserAsync(Guid userId)
    {
        try
        {
            const string sql = "DELETE FROM users WHERE user_id = @UserId;";
            return await dbContext.Connection.ExecuteAsync(sql, new { userId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<IEnumerable<Recruiter>> GetAllRecruitersAsync()
    {
        try
        {
            const string sql = "SELECT * FROM recruiters";
            return await dbContext.Connection.QueryAsync<Recruiter>(sql);
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<Recruiter?> GetRecruiterByIdAsync(Guid recruiterId)
    {
        try
        {
            const string sql = "SELECT * FROM recruiters WHERE recruiter_id = @RecruiterId;";
            return await dbContext.Connection.QuerySingleOrDefaultAsync<Recruiter>(sql, new { recruiterId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> CreateRecruiterAsync(RecruiterRegistration recruiterRegistration)
    {
        try
        {
            using var transaction = dbContext.Connection.BeginTransaction();

            try
            {
                // Insert recruiter into unified_users

                const string usersSql =
                    """
                    INSERT INTO unified_users (user_id, email, user_type, created_at)
                    VALUES (@RecruiterId, @Email, @UserType, @CreatedAt); 
                    """;

                await dbContext.Connection.ExecuteAsync(usersSql,
                    new
                    {
                        recruiterRegistration.Recruiter.RecruiterId,
                        recruiterRegistration.Recruiter.Email,
                        UserType = "recruiter",
                        CreatedAt = DateTime.Now
                    },
                    transaction);

                // Insert recruiter
                const string recruiterSql =
                    """
                    INSERT INTO recruiters (recruiter_id, first_name,
                    last_name, email, company, phone_number, notes, created_at)
                    VALUES (@RecruiterId, @FirstName, @LastName, @Email,
                    @Company, @PhoneNumber, @Notes, @CreatedAt);
                    """;

                await dbContext.Connection.ExecuteAsync(recruiterSql, recruiterRegistration.Recruiter, transaction);

                // Insert recruiter's credentials
                const string authSql =
                    """
                        INSERT INTO user_authentication (auth_id, user_id, password_hash)
                        VALUES (@AuthId, @UserId, @PasswordHash);
                    """;

                recruiterRegistration.UserAuthentication.UserId = recruiterRegistration.Recruiter.RecruiterId;
                
                await dbContext.Connection.ExecuteAsync(
                    authSql,
                    new
                    {
                        recruiterRegistration.UserAuthentication.AuthId,
                        recruiterRegistration.UserAuthentication.UserId,
                        PasswordHash = PasswordService.HashPassword(recruiterRegistration.UserAuthentication.Password)
                    },
                    transaction);

                // Insert user into registered accounts
                const string registeredSql = """
                                             INSERT INTO registered_accounts (recruiter_id, created_at)
                                             VALUES (@RecruiterId, @CreatedAt)
                                             """;
                await dbContext.Connection.ExecuteAsync(registeredSql,
                    new { recruiterRegistration.Recruiter.RecruiterId, CreatedAt = DateTime.UtcNow }, transaction);

                transaction.Commit();

                return 1;
            }
            catch
            {
                if (transaction.Connection != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> UpdateRecruiterAsync(Guid recruiterId, RecruiterUpdate recruiter)
    {
        try
        {
            const string sql =
                """
                UPDATE recruiters SET first_name = @FirstName,
                last_name = @LastName, phone_number = @PhoneNumber,
                company = @Company, notes = @Notes
                WHERE recruiter_id = @RecruiterId;
                """;
            return await dbContext.Connection.ExecuteAsync(sql, new
            {
                RecruiterId = recruiterId,
                recruiter
            });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> DeleteRecruiterAsync(Guid recruiterId)
    {
        try
        {
            using var transaction = dbContext.Connection.BeginTransaction();

            try
            {
                const string sqlVacancies = "DELETE FROM vacancies WHERE recruiter_id = @RecruiterId";
                await dbContext.Connection.ExecuteAsync(sqlVacancies, new { RecruiterId = recruiterId }, transaction);
                
                const string sql = "DELETE FROM recruiters WHERE recruiter_id = @RecruiterId;";
                await dbContext.Connection.ExecuteAsync(sql, new { RecruiterId = recruiterId }, transaction);
                
                transaction.Commit();

                return 1;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<int> ResetPasswordAsync(UserAuthentication userAuthentication)
    {
        try
        {
            const string sql = "UPDATE user_authentication SET password_hash = @NewPassword WHERE user_id = @UserId;";

            var passwordHash = PasswordService.HashPassword(userAuthentication.Password);
            
            return await dbContext.Connection.ExecuteAsync(sql,
                new { NewPassword = passwordHash, userAuthentication.UserId });
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }

    public async Task<ValidateUserResult> ValidateUserAsync(LoginRegisterModel loginRegisterModel)
    {
        try
        {
            using var transaction = dbContext.Connection.BeginTransaction();

            try
            {
                const string unifiedUserSql = "SELECT * FROM unified_users WHERE email = @Email;";
                var uniUser =
                    await dbContext.Connection.QuerySingleOrDefaultAsync<UnifiedUser>(
                        unifiedUserSql,
                        new { loginRegisterModel.Email },
                        transaction);

                if (uniUser is null)
                {
                    transaction.Commit();
                    return new ValidateUserResult(null, null, false);
                }

                const string userAuthSql = "SELECT password_hash FROM user_authentication WHERE user_id = @UserId;";
                var passwordHash =
                    await dbContext.Connection.QuerySingleOrDefaultAsync<string>(
                        userAuthSql,
                        new { uniUser.UserId },
                        transaction) ?? string.Empty;

                var isValid = PasswordService.VerifyPassword(loginRegisterModel.Password, passwordHash);
                                
                transaction.Commit();
                
                return new ValidateUserResult(uniUser.UserId, uniUser.UserType, isValid);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (PostgresException e)
        {
            throw DatabaseExceptionHandler.CatchDatabaseException(e);
        }
    }
}