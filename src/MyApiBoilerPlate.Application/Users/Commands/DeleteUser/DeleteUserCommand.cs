using ErrorOr;
using Mediator;

namespace MyApiBoilerPlate.Application.Users.Commands.DeleteUser
{
  public sealed record DeleteUserCommand(int UserId) : IRequest<ErrorOr<bool>>;
}