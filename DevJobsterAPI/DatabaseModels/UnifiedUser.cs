using DevJobsterAPI.Common;

namespace DevJobsterAPI.DatabaseModels;

public record UnifiedUser(Guid UserId, string Email, UserType UserType, DateTime CreatedAt);