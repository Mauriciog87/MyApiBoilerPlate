using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyApiBoilerPlate.API.Pipelines;

internal sealed partial class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IWebHostEnvironment environment
) : IExceptionHandler
{
  [LoggerMessage(
      EventId = 1,
      Level = LogLevel.Error,
      Message = "Unhandled exception of type {ExceptionType} occurred. TraceId: {TraceId}")]
  private partial void LogUnhandledException(
      string exceptionType,
      string traceId,
      Exception exception);

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    LogUnhandledException(
        exception.GetType().Name,
        httpContext.TraceIdentifier,
        exception);

    if (httpContext.Response.HasStarted)
    {
      logger.LogWarning(
          "Cannot write error response. Response has already started for TraceId: {TraceId}",
          httpContext.TraceIdentifier);
      return false;
    }

    (int statusCode, string title, string typeUri) = MapExceptionToResponse(exception);

    httpContext.Response.StatusCode = statusCode;
    httpContext.Response.ContentType = "application/problem+json";

    var problemDetails = CreateProblemDetails(
        statusCode,
        title,
        typeUri,
        httpContext);

    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }

  private ProblemDetails CreateProblemDetails(
      int statusCode,
      string title,
      string typeUri,
      HttpContext httpContext)
  {
    return new ProblemDetails
    {
      Status = statusCode,
      Type = typeUri,
      Title = title,
      Detail = environment.IsDevelopment()
            ? "See logs for details."
            : "An error occurred processing your request.",
      Instance = httpContext.Request.Path,
      Extensions = new Dictionary<string, object?>
      {
        ["traceId"] = httpContext.TraceIdentifier,
        ["timestamp"] = DateTime.UtcNow
      }
    };
  }

  private static (int StatusCode, string Title, string TypeUri) MapExceptionToResponse(Exception exception)
  {
    return exception switch
    {
      ArgumentNullException => (StatusCodes.Status400BadRequest, "Bad Request", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
      ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
      InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict", "https://tools.ietf.org/html/rfc7231#section-6.5.8"),
      UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", "https://tools.ietf.org/html/rfc7235#section-3.1"),
      NotImplementedException => (StatusCodes.Status501NotImplemented, "Not Implemented", "https://tools.ietf.org/html/rfc7231#section-6.6.2"),
      TimeoutException => (StatusCodes.Status408RequestTimeout, "Request Timeout", "https://tools.ietf.org/html/rfc7231#section-6.5.7"),
      KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found", "https://tools.ietf.org/html/rfc7231#section-6.5.4"),
      _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "https://tools.ietf.org/html/rfc7231#section-6.6.1")
    };
  }
}