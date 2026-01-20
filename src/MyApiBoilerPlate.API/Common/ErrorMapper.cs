using ErrorOr;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyApiBoilerPlate.API.Common
{
  /// <summary>
  /// Centralized error mapping service that handles conversion of ErrorOr errors to HTTP status codes.
  /// This ensures consistent error handling across the application.
  /// </summary>
  public static class ErrorMapper
  {
    /// <summary>
    /// Maps an ErrorOr error to an HTTP status code.
    /// </summary>
    public static int MapErrorToStatusCode(Error error)
    {
      return error.Type switch
      {
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
      };
    }

    /// <summary>
    /// Maps a collection of ErrorOr errors to an HTTP status code.
    /// Uses the first error's type to determine the status code.
    /// </summary>
    public static int MapErrorsToStatusCode(List<Error> errors)
    {
      return errors.Count is 0 ? StatusCodes.Status500InternalServerError : MapErrorToStatusCode(errors[0]);
    }

    /// <summary>
    /// Maps exception types to HTTP status codes for general exception handling.
    /// Used by the GlobalExceptionHandler for unhandled exceptions.
    /// </summary>
    public static int MapExceptionToStatusCode(Exception exception)
    {
      return exception switch
      {
        ArgumentNullException => StatusCodes.Status400BadRequest,
        ArgumentException => StatusCodes.Status400BadRequest,
        InvalidOperationException => StatusCodes.Status409Conflict,
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        NotImplementedException => StatusCodes.Status501NotImplemented,
        TimeoutException => StatusCodes.Status408RequestTimeout,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
      };
    }

    /// <summary>
    /// Maps exception types to problem detail title and type URI.
    /// </summary>
    public static (string Title, string TypeUri) MapExceptionToResponse(Exception exception)
    {
      return exception switch
      {
        ArgumentNullException => ("Bad Request", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
        ArgumentException => ("Bad Request", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
        InvalidOperationException => ("Conflict", "https://tools.ietf.org/html/rfc7231#section-6.5.8"),
        UnauthorizedAccessException => ("Unauthorized", "https://tools.ietf.org/html/rfc7235#section-3.1"),
        NotImplementedException => ("Not Implemented", "https://tools.ietf.org/html/rfc7231#section-6.6.2"),
        TimeoutException => ("Request Timeout", "https://tools.ietf.org/html/rfc7231#section-6.5.7"),
        KeyNotFoundException => ("Not Found", "https://tools.ietf.org/html/rfc7231#section-6.5.4"),
        _ => ("Internal Server Error", "https://tools.ietf.org/html/rfc7231#section-6.6.1")
      };
    }

    /// <summary>
    /// Creates a ModelStateDictionary from validation errors for validation problem responses.
    /// </summary>
    public static ModelStateDictionary CreateModelStateDictionary(List<Error> errors)
    {
      var modelStateDictionary = new ModelStateDictionary();

      foreach (Error error in errors)
      {
        modelStateDictionary.AddModelError(error.Code, error.Description);
      }

      return modelStateDictionary;
    }
  }
}
