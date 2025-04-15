namespace DevJobsterAPI.DatabaseModels.RequestModels.Recruiter;

public record RecruiterView(string FirstName, string LastName, string? Notes, string Company, string PhoneNumber);