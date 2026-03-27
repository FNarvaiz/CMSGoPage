using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiKeyMiddleware> _logger;
    private const string ApiKeyHeader = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        if (!context.Request.Path.StartsWithSegments("/api/public"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var apiKeyValue))
        {
            _logger.LogWarning("Public API request without API Key: {Path}", context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "API Key requerida." });
            return;
        }

        var apiKey = apiKeyValue.ToString();
        var site = await db.Sites.FirstOrDefaultAsync(s => s.ApiKey == apiKey && s.IsActive);

        if (site is null)
        {
            _logger.LogWarning("Public API request with invalid API Key: {Path}", context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "API Key inválida o sitio inactivo." });
            return;
        }

        context.Items["SiteId"] = site.Id;
        context.Items["SiteSlug"] = site.Slug;

        await _next(context);
    }
}
