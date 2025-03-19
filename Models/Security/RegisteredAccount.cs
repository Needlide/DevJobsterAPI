using System.ComponentModel.DataAnnotations.Schema;
using DevJobsterAPI.Models.Admin;

namespace DevJobsterAPI.Models.Security;

public class RegisteredAccount
{
    public int RegisteredAccountId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RecruiterId { get; set; }
    public bool Checked { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    public User.User? User { get; set; }
    public Recruiter.Recruiter? Recruiter { get; set; }
    public List<AdminRegisteredAccount> AdminRegisteredAccounts { get; set; } = [];


    [NotMapped]
    public Guid? AccountId => UserId ?? RecruiterId;

    [NotMapped]
    public string? AccountType
    {
        get
        {
            if (UserId.HasValue) return "User";
            return RecruiterId.HasValue ? "Recruiter" : null;
        }
    }
}