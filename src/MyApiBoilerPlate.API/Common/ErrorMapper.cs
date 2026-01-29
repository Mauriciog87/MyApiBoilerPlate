using ErrorOr;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyApiBoilerPlate.API.Common
{
  /// <summary>
  /// Centralized error mapping service that handles conversion of ErrorOr errors to HTTP status codes.
  /// This ensures consistent error handling across the application.
  /// </summary>
  /// <remarks>
  /// The mapper is extensible - you can register custom error type mappings using 
  /// <see cref="RegisterErrorTypeMapping"/> and custom exception mappings using
  /// <see cref="RegisterExceptionMapping{TException}"/>.
  /// </remarks>
  public static class ErrorMapper
  {
    /// <summary>
    /// Mapping of ErrorType to HTTP status codes. Can be extended at startup.
    /// </summary>
    private static readonly Dictionary<ErrorType, int> ErrorTypeToStatusCode = new()
    {
      [ErrorType.Conflict] = StatusCodes.Status409Conflict,
      [ErrorType.Validation] = StatusCodes.Status400BadRequest,
      [ErrorType.NotFound] = StatusCodes.Status404NotFound,
      [ErrorType.Unauthorized] = StatusCodes.Status401Unauthorized,
      [ErrorType.Forbidden] = StatusCodes.Status403Forbidden,
      [ErrorType.Failure] = StatusCodes.Status500InternalServerError,
      [ErrorType.Unexpected] = StatusCodes.Status500InternalServerError
    };

    /// <summary>
    /// Mapping of custom error numeric types to HTTP status codes.
    /// Use this for custom ErrorOr error types created with Error.Custom().
    /// </summary>
    private static readonly Dictionary<int, int> CustomErrorTypeToStatusCode = new();

    /// <summary>
    /// Mapping of exception types to HTTP status codes.
    /// </summary>
    private static readonly Dictionary<Type, int> ExceptionTypeToStatusCode = new()
    {
      [typeof(ArgumentNullException)] = StatusCodes.Status400BadRequest,
      [typeof(ArgumentException)] = StatusCodes.Status400BadRequest,
      [typeof(InvalidOperationException)] = StatusCodes.Status409Conflict,
      [typeof(UnauthorizedAccessException)] = StatusCodes.Status401Unauthorized,
      [typeof(NotImplementedException)] = StatusCodes.Status501NotImplemented,
      [typeof(TimeoutException)] = StatusCodes.Status408RequestTimeout,
      [typeof(KeyNotFoundException)] = StatusCodes.Status404NotFound
    };

    /// <summary>
    /// Mapping of exception types to problem detail responses.
    /// </summary>
    private static readonly Dictionary<Type, (string Title, string TypeUri)> ExceptionTypeToResponse = new()
    {
      [typeof(ArgumentNullException)] = ("Bad Request", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
      [typeof(ArgumentException)] = ("Bad Request", "https://tools.ietf.org/html/rfc7231#section-6.5.1"),
      [typeof(InvalidOperationException)] = ("Conflict", "https://tools.ietf.org/html/rfc7231#section-6.5.8"),
      [typeof(UnauthorizedAccessException)] = ("Unauthorized", "https://tools.ietf.org/html/rfc7235#section-3.1"),
      [typeof(NotImplementedException)] = ("Not Implemented", "https://tools.ietf.org/html/rfc7231#section-6.6.2"),
      [typeof(TimeoutException)] = ("Request Timeout", "https://tools.ietf.org/html/rfc7231#section-6.5.7"),
      [typeof(KeyNotFoundException)] = ("Not Found", "https://tools.ietf.org/html/rfc7231#section-6.5.4")
    };

    private static readonly (string Title, string TypeUri) DefaultExceptionResponse = 
      ("Internal Server Error", "https://tools.ietf.org/html/rfc7231#section-6.6.1");

    /// <summary>
    /// Registers a custom mapping from ErrorType to HTTP status code.
    /// Call this at application startup to extend the default mappings.
    /// </summary>
    /// <param name="errorType">The ErrorType to map.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    public static void RegisterErrorTypeMapping(ErrorType errorType, int statusCode)
    {
      ErrorTypeToStatusCode[errorType] = statusCode;
    }

    /// <summary>
    /// Registers a custom mapping for errors created with Error.Custom(numericType, ...).
    /// </summary>
    /// <param name="customNumericType">The numeric type used when creating the custom error.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    public static void RegisterCustomErrorTypeMapping(int customNumericType, int statusCode)
    {
      CustomErrorTypeToStatusCode[customNumericType] = statusCode;
    }

    /// <summary>
    /// Registers a custom exception type mapping.
    /// </summary>
    /// <typeparam name="TException">The exception type to map.</typeparam>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <param name="title">The problem detail title.</param>
    /// <param name="typeUri">The problem detail type URI.</param>
    public static void RegisterExceptionMapping<TException>(
      int statusCode, 
      string title, 
      string typeUri) where TException : Exception
    {
      ExceptionTypeToStatusCode[typeof(TException)] = statusCode;
      ExceptionTypeToResponse[typeof(TException)] = (title, typeUri);
    }

    /// <summary>
    /// Maps an ErrorOr error to an HTTP status code.
    /// </summary>
    public static int MapErrorToStatusCode(Error error)
    {
      // First check custom numeric error types (for Error.Custom())
      if (CustomErrorTypeToStatusCode.TryGetValue(error.NumericType, out int customStatusCode))
      {
        return customStatusCode;
      }

      // Then check standard error types
      if (ErrorTypeToStatusCode.TryGetValue(error.Type, out int statusCode))
      {
        return statusCode;
      }

      // Default fallback
      return StatusCodes.Status500InternalServerError;
    }

    /// <summary>
    /// Maps a collection of ErrorOr errors to an HTTP status code.
    /// Uses the first error's type to determine the status code.
    /// </summary>
    public static int MapErrorsToStatusCode(List<Error> errors)
    {
      return errors.Count is 0 
        ? StatusCodes.Status500InternalServerError 
        : MapErrorToStatusCode(errors[0]);
    }

    /// <summary>
    /// Maps exception types to HTTP status codes for general exception handling.
    /// Used by the GlobalExceptionHandler for unhandled exceptions.
    /// </summary>
    public static int MapExceptionToStatusCode(Exception exception)
    {
            Type exceptionType = exception.GetType();

      // Check exact type match first
      if (ExceptionTypeToStatusCode.TryGetValue(exceptionType, out int statusCode))
      {
        return statusCode;
      }

      // Check base types
      foreach (KeyValuePair<Type, int> mapping in ExceptionTypeToStatusCode)
      {
        if (mapping.Key.IsAssignableFrom(exceptionType))
        {
          return mapping.Value;
        }
      }

      return StatusCodes.Status500InternalServerError;
    }

    /// <summary>
    /// Maps exception types to problem detail title and type URI.
    /// </summary>
    public static (string Title, string TypeUri) MapExceptionToResponse(Exception exception)
    {
            Type exceptionType = exception.GetType();

      // Check exact type match first
      if (ExceptionTypeToResponse.TryGetValue(exceptionType, out (string Title, string TypeUri) response))
      {
        return response;
      }

      // Check base types
      foreach (KeyValuePair<Type, (string Title, string TypeUri)> mapping in ExceptionTypeToResponse)
      {
        if (mapping.Key.IsAssignableFrom(exceptionType))
        {
          return mapping.Value;
        }
      }

      return DefaultExceptionResponse;
    }

    /// <summary>
    /// Creates a ModelStateDictionary from validation errors for validation problem responses.
    /// </summary>
    public static ModelStateDictionary CreateModelStateDictionary(List<Error> errors)
    {
            ModelStateDictionary modelStateDictionary = new ModelStateDictionary();

      foreach (Error error in errors)
      {
        modelStateDictionary.AddModelError(error.Code, error.Description);
      }

      return modelStateDictionary;
    }
  }
}
