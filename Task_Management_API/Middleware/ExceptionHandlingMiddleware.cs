namespace Task_Management_API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, Task_Management.Infrastructure.TaskDbContext dbContext)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception for {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            try
            {
                var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                    ?? context.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

                int? userId = int.TryParse(userIdClaim, out var parsedUserId)
                    ? parsedUserId
                    : null;

                dbContext.AuditLogs.Add(new Task_Management.Domain.AuditLog
                {
                    UserId = userId,
                    UserEmail = context.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                        ?? context.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)?.Value,
                    Message = ex.Message,
                    Description = "An unexpected error occurred while processing the request.",
                    ExceptionType = ex.GetType().Name,
                    StackTrace = ex.StackTrace,
                    RequestPath = context.Request.Path,
                    HttpMethod = context.Request.Method,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                    CreatedAt = DateTime.UtcNow
                });

                await dbContext.SaveChangesAsync();
            }
            catch (Exception auditException)
            {
                _logger.LogError(auditException, "Failed to save an audit record for an unhandled exception.");
            }

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "An unexpected error occurred."
                });
            }
        }
    }
}
