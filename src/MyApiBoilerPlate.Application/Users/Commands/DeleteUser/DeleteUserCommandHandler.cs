using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Commands.DeleteUser
{
  public sealed class DeleteUserCommandHandler(IUserRepository userRepository)
      : IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
  {
    public async ValueTask<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
      User? existingUser = await userRepository.GetUserById(request.UserId, cancellationToken);

      if (existingUser is null)
      {
        return Application.Common.Errors.Errors.User.NotFoundById(request.UserId);
      }

      await userRepository.DeleteUser(request.UserId, cancellationToken);
      return true;
    }
  }
}
