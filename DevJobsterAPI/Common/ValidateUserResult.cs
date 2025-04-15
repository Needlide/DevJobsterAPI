namespace DevJobsterAPI.Common;

public record ValidateUserResult(Guid? UserId, UserType? UserType, bool Success);