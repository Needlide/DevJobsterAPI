using DevJobsterAPI.Models.Recruiter;
using DevJobsterAPI.Models.User;

namespace DevJobsterAPI.Database.Abstract;

public interface IUserManagementService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid userId);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);

    Task<IEnumerable<Recruiter>> GetAllRecruitersAsync();
    Task<Recruiter> GetRecruiterByIdAsync(Guid recruiterId);
    Task CreateRecruiterAsync(Recruiter recruiter);
    Task UpdateRecruiterAsync(Recruiter recruiter);
    Task DeleteRecruiterAsync(Guid recruiterId);

    Task AuthenticateUserAsync(Guid userId, string password);
    Task ResetPasswordAsync(Guid userId, string newPassword);
    Task ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
}