using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;

namespace MyApiBoilerPlate.Application.Users.Commands.DeleteUser
{
  public class DeleteUserCommandHandler(IUserRepository userRepository)
      : IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
  {
    public async ValueTask<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
      var existingUser = await userRepository.GetUserById(request.UserId, cancellationToken);

      if (existingUser is null)
      {
        return Application.Common.Errors.Errors.User.NotFound;
      }

      await userRepository.DeleteUser(request.UserId, cancellationToken);
      return true;
    }
  }
}