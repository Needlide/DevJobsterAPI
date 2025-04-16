using DevJobsterAPI.Common;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Security;

public record RegisteredAccountShortView(
    string FirstName,
    string LastName,
    UserType Role,
    Guid UserId,
    DateTime CreatedAt);