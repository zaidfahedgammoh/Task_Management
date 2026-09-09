namespace Task_Management.Domain;

public class AuditLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }
    public string? UserEmail { get; set; }

    public string Message { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string ExceptionType { get; set; } = string.Empty;
    public string? StackTrace { get; set; }

    public string RequestPath { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;

    public int StatusCode { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
}