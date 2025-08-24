namespace MyApiBoilerPlate.Requests.Users
{
    public sealed record UpdateUserRequest(
        int UserId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTime DateOfBirth,
        bool IsActive
    );
}