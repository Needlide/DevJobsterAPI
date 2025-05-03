using DevJobsterAPI.DatabaseModels.RequestModels.Admin;

namespace DevJobsterAPI.DatabaseModels.RequestModels.Security;

public record LogView(string Body, AdminView? Admin, DateTime CreatedAt);