namespace MyApiBoilerPlate.Requests.Users
{
  public sealed record UpdateUserRequest(
      string FirstName,
      string LastName,
      string Email,
      string PhoneNumber,
      DateTime DateOfBirth,
      bool IsActive
  );
}