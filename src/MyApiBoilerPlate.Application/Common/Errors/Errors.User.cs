namespace MyApiBoilerPlate.Application.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static ErrorOr.Error NotFound => ErrorOr.Error.NotFound(
                code: "User.NotFound",
                description: "User not found."
            );
            public static ErrorOr.Error InvalidCredentials => ErrorOr.Error.Validation(
                code: "User.InvalidCredentials",
                description: "The provided credentials are invalid."
            );
            public static ErrorOr.Error AlreadyExists => ErrorOr.Error.Conflict(
                code: "User.AlreadyExists",
                description: "A user with the given email already exists."
            );
            public static ErrorOr.Error Unauthorized => ErrorOr.Error.Unauthorized(
                code: "User.Unauthorized",
                description: "You are not authorized to perform this action."
            );
        }
    }
}