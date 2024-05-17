using Infrastructure.Helpers.Responses;

namespace Infrastructure.Factories;

public class ResponseFactory
{
    public static ResponseResult Ok()
    {
        return new ResponseResult
        {

            Message = "Succeded",
            StatusCode = ResultStatus.OK,
        };
    }
    public static ResponseResult Ok(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Succeded",
            StatusCode = ResultStatus.OK,
        };
    }

    public static ResponseResult Ok(object obj, string? message = null)
    {
        return new ResponseResult
        {
            ContentResult = obj,
            Message = message ?? "Succeded",
            StatusCode = ResultStatus.OK,
        };
    }

    public static ResponseResult Error(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Something went wrong, please try again.",
            StatusCode = ResultStatus.ERROR,
        };
    }

    public static ResponseResult NotFound(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Not found",
            StatusCode = ResultStatus.NOT_FOUND,
        };
    }

    public static ResponseResult Exists(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "Already exists",
            StatusCode = ResultStatus.EXISTS,
        };
    }

    public static ResponseResult UnAuthorized(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "The server encountered an unexpected condition",
            StatusCode = ResultStatus.UNAUTHORIZED,
        };
    }

    public static ResponseResult ServerError(string? message = null)
    {
        return new ResponseResult
        {
            Message = message ?? "The server encountered an unexpected condition",
            StatusCode = ResultStatus.SERVER_ERROR,
        };
    }
}
