using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyApiBoilerPlate.API.Pipeline;

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

    (int statusCode, string title) = MapExceptionToResponse(exception);

    httpContext.Response.StatusCode = statusCode;
    httpContext.Response.ContentType = "application/problem+json";

    var problemDetails = CreateProblemDetails(
        exception,
        statusCode,
        title,
        httpContext);

    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }

  private ProblemDetails CreateProblemDetails(
      Exception exception,
      int statusCode,
      string title,
      HttpContext httpContext)
  {
    return new ProblemDetails
    {
      Status = statusCode,
      Type = exception.GetType().Name,
      Title = title,
      Detail = environment.IsDevelopment()
            ? exception.Message
            : "An error occurred processing your request.",
      Instance = httpContext.Request.Path,
      Extensions = new Dictionary<string, object?>
      {
        ["traceId"] = httpContext.TraceIdentifier,
        ["timestamp"] = DateTime.UtcNow
      }
    };
  }

  private static (int StatusCode, string Title) MapExceptionToResponse(Exception exception)
  {
    return exception switch
    {
      ArgumentNullException => (StatusCodes.Status400BadRequest, "Bad Request"),
      ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
      InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict"),
      UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
      NotImplementedException => (StatusCodes.Status501NotImplemented, "Not Implemented"),
      TimeoutException => (StatusCodes.Status408RequestTimeout, "Request Timeout"),
      KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
      _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
    };
  }
}