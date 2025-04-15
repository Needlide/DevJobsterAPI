namespace DevJobsterAPI.DatabaseModels.RequestModels.Security;

public record RegisteredAccountShortView(string FirstName, string LastName, bool Role, DateTime CreatedAt);