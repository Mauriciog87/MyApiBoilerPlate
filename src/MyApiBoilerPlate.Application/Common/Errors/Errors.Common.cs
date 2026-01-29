namespace MyApiBoilerPlate.Application.Common.Errors
{
  /// <summary>
  /// Common error definitions shared across the application.
  /// </summary>
  public static partial class Errors
  {
    /// <summary>
    /// Generic, infrastructure-level, and cross-cutting error definitions.
    /// Use these for errors that are not specific to any particular domain entity.
    /// </summary>
    public static class Common
    {
      private const string Prefix = "General";

      /// <summary>
      /// Error for unexpected/unhandled situations.
      /// </summary>
      public static ErrorOr.Error Unexpected => ErrorOr.Error.Unexpected(
          code: $"{Prefix}.Unexpected",
          description: "An unexpected error occurred."
      );

      /// <summary>
      /// Error for unexpected situations with a custom message.
      /// </summary>
      /// <param name="message">Custom error message describing the unexpected situation.</param>
      public static ErrorOr.Error UnexpectedWithMessage(string message) => ErrorOr.Error.Unexpected(
          code: $"{Prefix}.Unexpected",
          description: message
      );

      /// <summary>
      /// Error for database operation failures.
      /// </summary>
      public static ErrorOr.Error DatabaseError => ErrorOr.Error.Failure(
          code: $"{Prefix}.DatabaseError",
          description: "A database error occurred while processing the request."
      );

      /// <summary>
      /// Error for database operation failures with details.
      /// </summary>
      /// <param name="operation">The database operation that failed (e.g., "insert", "update", "delete").</param>
      /// <param name="entity">The entity type involved in the operation.</param>
      public static ErrorOr.Error DatabaseErrorWithDetails(string operation, string entity) => ErrorOr.Error.Failure(
          code: $"{Prefix}.DatabaseError",
          description: $"Failed to {operation} {entity}.",
          metadata: new Dictionary<string, object>
          {
            ["Operation"] = operation,
            ["Entity"] = entity
          }
      );

      /// <summary>
      /// Error for external service/API failures.
      /// </summary>
      public static ErrorOr.Error ExternalServiceError => ErrorOr.Error.Failure(
          code: $"{Prefix}.ExternalServiceError",
          description: "An external service is currently unavailable."
      );

      /// <summary>
      /// Error for external service failures with service name.
      /// </summary>
      /// <param name="serviceName">The name of the external service that failed.</param>
      public static ErrorOr.Error ExternalServiceErrorWithName(string serviceName) => ErrorOr.Error.Failure(
          code: $"{Prefix}.ExternalServiceError",
          description: $"The external service '{serviceName}' is currently unavailable.",
          metadata: new Dictionary<string, object> { ["ServiceName"] = serviceName }
      );

      /// <summary>
      /// Error for timeout situations.
      /// </summary>
      public static ErrorOr.Error Timeout => ErrorOr.Error.Failure(
          code: $"{Prefix}.Timeout",
          description: "The operation timed out."
      );

      /// <summary>
      /// Error for timeout situations with operation details.
      /// </summary>
      /// <param name="operation">The operation that timed out.</param>
      /// <param name="timeoutSeconds">The timeout duration in seconds.</param>
      public static ErrorOr.Error TimeoutWithDetails(string operation, int timeoutSeconds) => ErrorOr.Error.Failure(
          code: $"{Prefix}.Timeout",
          description: $"The operation '{operation}' timed out after {timeoutSeconds} seconds.",
          metadata: new Dictionary<string, object>
          {
            ["Operation"] = operation,
            ["TimeoutSeconds"] = timeoutSeconds
          }
      );

      /// <summary>
      /// Error for configuration-related issues.
      /// </summary>
      /// <param name="configKey">The configuration key that is missing or invalid.</param>
      public static ErrorOr.Error ConfigurationError(string configKey) => ErrorOr.Error.Failure(
          code: $"{Prefix}.ConfigurationError",
          description: $"Configuration error: '{configKey}' is missing or invalid.",
          metadata: new Dictionary<string, object> { ["ConfigKey"] = configKey }
      );

      /// <summary>
      /// Error for rate limiting scenarios.
      /// </summary>
      public static ErrorOr.Error RateLimitExceeded => ErrorOr.Error.Failure(
          code: $"{Prefix}.RateLimitExceeded",
          description: "Too many requests. Please try again later."
      );

      /// <summary>
      /// Error for rate limiting with retry information.
      /// </summary>
      /// <param name="retryAfterSeconds">Seconds until the client can retry.</param>
      public static ErrorOr.Error RateLimitExceededWithRetry(int retryAfterSeconds) => ErrorOr.Error.Failure(
          code: $"{Prefix}.RateLimitExceeded",
          description: $"Too many requests. Please try again in {retryAfterSeconds} seconds.",
          metadata: new Dictionary<string, object> { ["RetryAfterSeconds"] = retryAfterSeconds }
      );

      /// <summary>
      /// Error for concurrency conflicts (optimistic locking).
      /// </summary>
      public static ErrorOr.Error ConcurrencyConflict => ErrorOr.Error.Conflict(
          code: $"{Prefix}.ConcurrencyConflict",
          description: "The resource was modified by another request. Please refresh and try again."
      );

      /// <summary>
      /// Error for file/resource not accessible.
      /// </summary>
      /// <param name="resourceName">The name of the resource that is not accessible.</param>
      public static ErrorOr.Error ResourceNotAccessible(string resourceName) => ErrorOr.Error.Failure(
          code: $"{Prefix}.ResourceNotAccessible",
          description: $"The resource '{resourceName}' is not accessible.",
          metadata: new Dictionary<string, object> { ["ResourceName"] = resourceName }
      );

      /// <summary>
      /// Error for operations not yet implemented.
      /// </summary>
      /// <param name="featureName">The name of the feature that is not implemented.</param>
      public static ErrorOr.Error NotImplemented(string featureName) => ErrorOr.Error.Failure(
          code: $"{Prefix}.NotImplemented",
          description: $"The feature '{featureName}' is not yet implemented.",
          metadata: new Dictionary<string, object> { ["FeatureName"] = featureName }
      );
    }
  }
}
