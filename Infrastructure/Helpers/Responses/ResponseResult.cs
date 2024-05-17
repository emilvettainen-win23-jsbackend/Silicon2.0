namespace Infrastructure.Helpers.Responses;

public enum ResultStatus
{
    OK = 200,
    CREATED = 201,

    ERROR = 400,
    UNAUTHORIZED = 401,
    FORBIDDEN = 403,
    NOT_FOUND = 404,
    EXISTS = 409,

    SERVER_ERROR = 500,
}

public class ResponseResult
{
    public ResultStatus StatusCode { get; set; }
    public object? ContentResult { get; set; }
    public string? Message { get; set; }
}
