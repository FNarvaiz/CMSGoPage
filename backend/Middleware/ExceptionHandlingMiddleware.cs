using Serilog;

namespace backend.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            InvalidOperationException => StatusCodes.Status409Conflict,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var traceId = context.TraceIdentifier;

        object response = _env.IsDevelopment()
            ? new { error = ex.Message, traceId, detail = ex.StackTrace }
            : new { error = GetPublicMessage(statusCode), traceId };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static string GetPublicMessage(int statusCode) => statusCode switch
    {
        409 => "Conflicto en la operacion.",
        401 => "No autorizado.",
        404 => "Recurso no encontrado.",
        400 => "Solicitud invalida.",
        _ => "Ocurrio un error interno. Por favor intente mas tarde."
    };
}
