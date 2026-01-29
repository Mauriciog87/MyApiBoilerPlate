using ErrorOr;
using Mediator;

namespace MyApiBoilerPlate.Application.Users.Commands.UpdateUser
{
  public sealed record UpdateUserCommand(
      int UserId,
      string FirstName,
      string LastName,
      string Email,
      string PhoneNumber,
      DateTime DateOfBirth,
      bool IsActive
  ) : IRequest<ErrorOr<bool>>;
}