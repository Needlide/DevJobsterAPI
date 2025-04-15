using System.ComponentModel.DataAnnotations.Schema;
using DevJobsterAPI.Common;
using DevJobsterAPI.DatabaseModels.Admin;

namespace DevJobsterAPI.DatabaseModels.Security;

public class RegisteredAccount
{
    private RegisteredAccount()
    {
    }

    public RegisteredAccount(bool isChecked, Guid? userId = null, Guid? recruiterId = null)
    {
        IsChecked = isChecked;
        UserId = userId;
        RecruiterId = recruiterId;

        CreatedAt = DateTime.UtcNow;
    }

    public RegisteredAccount(bool isChecked, User.User? user = null,
        Recruiter.Recruiter? recruiter = null)
    {
        IsChecked = isChecked;
        User = user;
        Recruiter = recruiter;
        UserId = user?.UserId;
        RecruiterId = recruiter?.RecruiterId;

        CreatedAt = DateTime.UtcNow;
    }

    public int RegisteredAccountId { get; init; }
    public Guid? UserId { get; init; }
    public Guid? RecruiterId { get; init; }
    public bool IsChecked { get; init; }
    public DateTime CreatedAt { get; init; }

    public User.User? User { get; init; }
    public Recruiter.Recruiter? Recruiter { get; init; }
    public List<AdminRegisteredAccount> AdminRegisteredAccounts { get; init; } = [];

    [NotMapped] public Guid? AccountId => UserId ?? RecruiterId;

    [NotMapped]
    public UserType? AccountType
    {
        get
        {
            if (UserId.HasValue) return UserType.User;
            return RecruiterId.HasValue ? UserType.Recruiter : null;
        }
    }
}