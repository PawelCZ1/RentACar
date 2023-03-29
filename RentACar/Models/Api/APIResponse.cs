using System.Net;

namespace Investments.Models.Api;

public class APIResponse
{
    public APIResponse()
    {
        ErrorMessages = new List<string>();
    }

    public APIResponse(HttpStatusCode statusCode, bool isSuccess, List<string>? errorMessages, object? result)
    {
        StatusCode = statusCode;
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        Result = result;
    }

    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } 
    public List<string>? ErrorMessages { get; set; }
    public object? Result { get; set; }
}