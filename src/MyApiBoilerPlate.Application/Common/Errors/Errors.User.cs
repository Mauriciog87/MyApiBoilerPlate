namespace MyApiBoilerPlate.Application.Common.Errors
{
    /// <summary>
    /// Centralized error definitions for the application.
    /// Each entity has its own nested static class with domain-specific errors.
    /// </summary>
    public static partial class Errors
    {
        /// <summary>
        /// User-related error definitions.
        /// </summary>
        public static class User
        {
            private const string Prefix = "User";

            /// <summary>
            /// Error returned when a user is not found.
            /// </summary>
            public static ErrorOr.Error NotFound => ErrorOr.Error.NotFound(
                code: $"{Prefix}.NotFound",
                description: "User not found."
            );

            /// <summary>
            /// Error returned when a user is not found, including the user ID for context.
            /// </summary>
            /// <param name="userId">The ID of the user that was not found.</param>
            public static ErrorOr.Error NotFoundById(int userId) => ErrorOr.Error.NotFound(
                code: $"{Prefix}.NotFound",
                description: $"User with ID '{userId}' was not found.",
                metadata: new Dictionary<string, object> { ["UserId"] = userId }
            );

            /// <summary>
            /// Error returned when credentials are invalid.
            /// </summary>
            public static ErrorOr.Error InvalidCredentials => ErrorOr.Error.Validation(
                code: $"{Prefix}.InvalidCredentials",
                description: "The provided credentials are invalid."
            );

            /// <summary>
            /// Error returned when a user with the given email already exists.
            /// </summary>
            public static ErrorOr.Error AlreadyExists => ErrorOr.Error.Conflict(
                code: $"{Prefix}.AlreadyExists",
                description: "A user with the given email already exists."
            );

            /// <summary>
            /// Error returned when a user with a specific email already exists.
            /// </summary>
            /// <param name="email">The email that already exists.</param>
            public static ErrorOr.Error AlreadyExistsByEmail(string email) => ErrorOr.Error.Conflict(
                code: $"{Prefix}.AlreadyExists",
                description: $"A user with email '{email}' already exists.",
                metadata: new Dictionary<string, object> { ["Email"] = email }
            );

            /// <summary>
            /// Error returned when user is not authorized to perform an action.
            /// </summary>
            public static ErrorOr.Error Unauthorized => ErrorOr.Error.Unauthorized(
                code: $"{Prefix}.Unauthorized",
                description: "You are not authorized to perform this action."
            );

            /// <summary>
            /// Error returned when user is not authorized, including the action for context.
            /// </summary>
            /// <param name="action">The action the user is not authorized to perform.</param>
            public static ErrorOr.Error UnauthorizedForAction(string action) => ErrorOr.Error.Unauthorized(
                code: $"{Prefix}.Unauthorized",
                description: $"You are not authorized to {action}.",
                metadata: new Dictionary<string, object> { ["Action"] = action }
            );
        }
    }
}