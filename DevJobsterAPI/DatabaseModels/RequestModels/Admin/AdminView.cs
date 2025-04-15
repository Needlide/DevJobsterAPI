using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Admin;

public record AdminView(string FirstName, string LastName, bool Role); // true - moderator, false - admin