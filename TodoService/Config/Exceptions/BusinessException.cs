using System.Net;

namespace TodoService.Config.Exceptions;

public class BusinessException(string errorCode, HttpStatusCode status): Exception(errorCode)
{
    public string ErrorCode { get; private set; } = errorCode;
    public HttpStatusCode Status { get; private set; } = status;
}