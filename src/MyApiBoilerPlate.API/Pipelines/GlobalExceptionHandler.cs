using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyApiBoilerPlate.API.Common;

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

    int statusCode = ErrorMapper.MapExceptionToStatusCode(exception);
    var (title, typeUri) = ErrorMapper.MapExceptionToResponse(exception);

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
}