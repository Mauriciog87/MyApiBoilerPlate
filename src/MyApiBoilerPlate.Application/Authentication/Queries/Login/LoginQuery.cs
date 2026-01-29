using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Authentication.Common;
using MyApiBoilerPlate.Application.Common.Interfaces.Authentication;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Application.Common.Errors;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Authentication.Queries.Login
{
  public record LoginQuery(
      string Email,
      string Password) : IRequest<ErrorOr<AuthenticationResult>>;

  public class LoginQueryHandler(
      IUserRepository userRepository,
      IJwtTokenGenerator jwtTokenGenerator,
      IPasswordHasher passwordHasher) : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
  {
    public async ValueTask<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
      // 1. Get user by email
      User? user = await userRepository.GetUserByEmail(query.Email, cancellationToken);
      if (user is null)
      {
        return Errors.User.InvalidCredentials;
      }

      // 2. Verify password
      if (!passwordHasher.VerifyPassword(query.Password, user.PasswordHash))
      {
        return Errors.User.InvalidCredentials;
      }

      // 3. Generate token
      string token = jwtTokenGenerator.GenerateToken(
          user.UserId,
          user.FirstName,
          user.LastName,
          user.Email);

      return new AuthenticationResult(user, token);
    }
  }
}
