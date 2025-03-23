using DevJobsterAPI.Models.Recruiter;
using DevJobsterAPI.Models.User;

namespace DevJobsterAPI.Database.Abstract;

public interface IUserManagementService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid userId);
    Task<int> CreateUserAsync(User user);
    Task<int> UpdateUserAsync(User user);
    Task<int> DeleteUserAsync(Guid userId);

    Task<IEnumerable<Recruiter>> GetAllRecruitersAsync();
    Task<Recruiter> GetRecruiterByIdAsync(Guid recruiterId);
    Task<int> CreateRecruiterAsync(Recruiter recruiter);
    Task<int> UpdateRecruiterAsync(Recruiter recruiter);
    Task<int> DeleteRecruiterAsync(Guid recruiterId);

    Task<int> ResetPasswordAsync(Guid userId, string newPassword);
}