using AuthService.Config.Exceptions;

namespace AuthService.Config.Middlewares;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger): IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BusinessException e)
        {
            context.Response.StatusCode = (int)e.Status;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { e.Message });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception occured");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { Message = "Internal Server Error" });
        }
    }
}