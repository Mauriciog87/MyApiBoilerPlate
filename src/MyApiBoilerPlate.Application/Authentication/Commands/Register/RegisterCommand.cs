using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Authentication.Common;
using MyApiBoilerPlate.Application.Common.Interfaces.Authentication;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Domain.Entities;
using MyApiBoilerPlate.Application.Common.Errors;

namespace MyApiBoilerPlate.Application.Authentication.Commands.Register
{
  public record RegisterCommand(
      string FirstName,
      string LastName,
      string Email,
      string PhoneNumber,
      string Password,
      DateTime DateOfBirth) : IRequest<ErrorOr<AuthenticationResult>>;

  public class RegisterCommandHandler(
      IUserRepository userRepository,
      IJwtTokenGenerator jwtTokenGenerator,
      IPasswordHasher passwordHasher) : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
  {
    public async ValueTask<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
      // 1. Check if user already exists
      if (await userRepository.GetUserByEmail(command.Email, cancellationToken) is not null)
      {
        return Errors.User.AlreadyExistsByEmail(command.Email);
      }

      // 2. Hash password
      string passwordHash = passwordHasher.HashPassword(command.Password);

      // 3. Create user
      User user = new User(
          command.FirstName,
          command.LastName,
          command.Email,
          command.PhoneNumber,
          passwordHash,
          command.DateOfBirth);

      User createdUser = await userRepository.CreateUser(user, cancellationToken);

      // 4. Generate token
      string token = jwtTokenGenerator.GenerateToken(
        createdUser.UserId,
        createdUser.FirstName,
        createdUser.LastName,
        createdUser.Email);

      return new AuthenticationResult(createdUser, token);
    }
  }
}
