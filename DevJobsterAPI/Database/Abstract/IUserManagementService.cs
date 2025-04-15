using DevJobsterAPI.Common;
using DevJobsterAPI.DatabaseModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using DevJobsterAPI.DatabaseModels.RequestModels.User;
using DevJobsterAPI.DatabaseModels.Security;
using DevJobsterAPI.DatabaseModels.User;

namespace DevJobsterAPI.Database.Abstract;

public interface IUserManagementService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<int> CreateUserAsync(UserRegistration userRegistration);
    Task<int> UpdateUserAsync(Guid userId, UserUpdate user);
    Task<int> DeleteUserAsync(Guid userId);

    Task<IEnumerable<Recruiter>> GetAllRecruitersAsync();
    Task<Recruiter?> GetRecruiterByIdAsync(Guid recruiterId);
    Task<int> CreateRecruiterAsync(RecruiterRegistration recruiterRegistration);
    Task<int> UpdateRecruiterAsync(Guid recruiterId, RecruiterUpdate recruiter);
    Task<int> DeleteRecruiterAsync(Guid recruiterId);

    Task<int> ResetPasswordAsync(UserAuthentication userAuthentication);
    Task<ValidateUserResult> ValidateUserAsync(LoginRegisterModel loginRegisterModel);
}