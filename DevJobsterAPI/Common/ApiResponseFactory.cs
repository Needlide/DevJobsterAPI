using DevJobsterAPI.ApiModels;

namespace DevJobsterAPI.Common;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ApiResponse<T> Fail<T>(string message, string errorCode, Dictionary<string, string[]>? validationErrors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = default,
            Message = message,
            ErrorCode = errorCode,
            ValidationErrors = validationErrors
        };
    }
}