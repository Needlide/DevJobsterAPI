using System.ComponentModel.DataAnnotations.Schema;
using DevJobsterAPI.Common;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Chat;

public record MessageView(
    string Body,
    DatabaseModels.Chat.Chat Chat,
    Guid ChatId,
    Guid? UserId = null,
    Guid? RecruiterId = null,
    DatabaseModels.User.User? User = null,
    DatabaseModels.Recruiter.Recruiter? Recruiter = null)
{
    [NotMapped] public Guid? SenderId => UserId ?? RecruiterId;

    [NotMapped]
    public UserType? SenderType => UserId.HasValue ? UserType.User : RecruiterId.HasValue ? UserType.Recruiter : null;
}