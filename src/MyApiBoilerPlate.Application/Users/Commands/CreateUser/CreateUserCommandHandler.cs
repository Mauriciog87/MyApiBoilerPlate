using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Commands.CreateUser
{
    public sealed class CreateUserCommandHandler(
        IUserRepository userRepository
    ) : IRequestHandler<CreateUserCommand, ErrorOr<UserCreatedResult>>
    {
        public async ValueTask<ErrorOr<UserCreatedResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool userExists = await userRepository.CheckIfUserExists(request.Email, cancellationToken);

            if (userExists)
            {
                return Application.Common.Errors.Errors.User.AlreadyExistsByEmail(request.Email);
            }

            // Create user using domain constructor
            User user = new User(
              request.FirstName,
              request.LastName,
              request.Email,
              request.PhoneNumber,
              request.DateOfBirth
            );

            User createdUser = await userRepository.CreateUser(user, cancellationToken);

            return new UserCreatedResult(createdUser.UserId, createdUser.Email);
        }
    }
}
