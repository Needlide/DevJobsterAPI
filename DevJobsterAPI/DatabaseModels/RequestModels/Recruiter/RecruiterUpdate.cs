namespace DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;

public record RecruiterUpdate(string FirstName, string LastName, string PhoneNumber, string Company, string? Notes);