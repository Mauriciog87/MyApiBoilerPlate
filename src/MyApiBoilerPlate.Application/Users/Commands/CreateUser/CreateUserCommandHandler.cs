using ErrorOr;
using MapsterMapper;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Commands.CreateUser
{
  public class CreateUserCommandHandler(
      IMapper mapper,
      IUserRepository userRepository
  ) : IRequestHandler<CreateUserCommand, ErrorOr<UserCreatedResult>>
  {
    public async ValueTask<ErrorOr<UserCreatedResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
      bool userExists = await userRepository.CheckIfUserExists(request.Email, cancellationToken);

      if (userExists)
      {
        return Application.Common.Errors.Errors.User.AlreadyExists;
      }

      User user = mapper.Map<User>(request);
      User createdUser = await userRepository.CreateUser(user, cancellationToken);

      return new UserCreatedResult(createdUser.UserId, createdUser.Id, createdUser.Email);
    }
  }
}