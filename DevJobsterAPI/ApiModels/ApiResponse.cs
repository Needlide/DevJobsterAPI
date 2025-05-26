namespace DevJobsterAPI.ApiModels;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public required T Data { get; set; }
    public string? Message { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, string[]>? ValidationErrors { get; set; }
}