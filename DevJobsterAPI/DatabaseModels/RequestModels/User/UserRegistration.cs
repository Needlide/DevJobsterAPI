using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.RequestModels.User;

public record UserRegistration(DatabaseModels.User.User User, UserAuthentication UserAuthentication);