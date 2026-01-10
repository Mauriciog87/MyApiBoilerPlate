namespace MyApiBoilerPlate.Application.Users.Common;

public sealed record UserResponse(
    int UserId,
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime DateOfBirth,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
