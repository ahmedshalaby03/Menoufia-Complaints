using ComplaintSystem.Application.Common;
using ComplaintSystem.Application.Services;
using System.Net;
using System.Text.Json;

namespace ComplaintSystem.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DuplicateComplaintException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 409; // Conflict - يبين للفرونت إنها نتيجة duplicate check
            await context.Response.WriteAsync(JsonSerializer.Serialize(ApiResponse<object>.Fail(ex.Message) is var r
                ? new { r.Success, r.Message, duplicates = ex.Result }
                : null));
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail("حدث خطأ غير متوقع"));
        }
    }
}
