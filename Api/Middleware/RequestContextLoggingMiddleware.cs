using Serilog.Context;

namespace Api.Middleware;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1812:Avoid uninstantiated internal classes",
    Justification = "Instantiated via ASP.NET Core middleware pipeline")]
internal sealed class RequestContextLoggingMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
        {
            return _next(httpContext);
        }
    }

    private static string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId);
        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}
