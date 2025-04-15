using System.ComponentModel.DataAnnotations.Schema;
using DevJobsterAPI.Common;
using DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;
using DevJobsterAPI.DatabaseModels.RequestModels.User;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Admin;

public record AdminRegisteredAccountView(
    string FirstName,
    string LastName,
    bool Checked,
    UserProfileView? UserProfile,
    RecruiterView? RecruiterProfile)
{
    [NotMapped]
    public UserType? AccountType
    {
        get
        {
            if (UserProfile == null && RecruiterProfile == null) return null;

            return RecruiterProfile == null ? UserType.User : UserType.Recruiter;
        }
    }
}