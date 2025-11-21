using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyApiBoilerPlate.API.Pipeline;

internal sealed partial class ValidationExceptionHandler(
    ILogger<ValidationExceptionHandler> logger
) : IExceptionHandler
{
  [LoggerMessage(
      EventId = 2,
      Level = LogLevel.Warning,
      Message = "Validation failed with {ErrorCount} validation error(s). TraceId: {TraceId}")]
  private partial void LogValidationException(
      int errorCount,
      string traceId,
      Exception exception);

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    if (exception is not ValidationException validationException)
    {
      return false;
    }

    Dictionary<string, string[]> errors = validationException.Errors
        .GroupBy(e => e.PropertyName)
        .ToDictionary(
            g => ToCamelCase(g.Key),
            g => g.Select(e => e.ErrorMessage).ToArray()
        );

    LogValidationException(
        errors.Sum(e => e.Value.Length),
        httpContext.TraceIdentifier,
        exception);

    if (httpContext.Response.HasStarted)
    {
      logger.LogWarning(
          "Cannot write validation error response. Response has already started for TraceId: {TraceId}",
          httpContext.TraceIdentifier);
      return false;
    }

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    httpContext.Response.ContentType = "application/problem+json";

    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status400BadRequest,
      Type = "ValidationFailure",
      Title = "One or more validation errors occurred",
      Detail = "The request contains invalid data. Please check the errors and try again.",
      Instance = httpContext.Request.Path,
      Extensions = new Dictionary<string, object?>
      {
        ["errors"] = errors,
        ["traceId"] = httpContext.TraceIdentifier,
        ["timestamp"] = DateTime.UtcNow
      }
    };

    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }

  private static string ToCamelCase(string propertyName)
  {
    if (string.IsNullOrEmpty(propertyName) || char.IsLower(propertyName[0]))
      return propertyName;

    return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
  }
}