using System.ComponentModel.DataAnnotations.Schema;
using DevJobsterAPI.Common;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Chat;

public record MessageView(
    string Body,
    Guid ChatId,
    Guid? UserId = null,
    Guid? RecruiterId = null)
{
    [NotMapped] public Guid? SenderId => UserId ?? RecruiterId;

    [NotMapped]
    public UserType? SenderType => UserId.HasValue ? UserType.User : RecruiterId.HasValue ? UserType.Recruiter : null;
}