namespace DevJobsterAPI.DatabaseModels.RequestModels.Security;

public record LogView(string Body, DatabaseModels.Admin.Admin Admin, DateTime CreatedAt);