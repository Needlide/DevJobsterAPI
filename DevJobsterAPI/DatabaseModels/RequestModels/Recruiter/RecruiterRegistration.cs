using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;

public record RecruiterRegistration(
    DatabaseModels.Recruiter.Recruiter Recruiter,
    UserAuthentication UserAuthentication);