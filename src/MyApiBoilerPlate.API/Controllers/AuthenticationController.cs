using ErrorOr;
using MapsterMapper;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApiBoilerPlate.Application.Authentication.Commands.Register;
using MyApiBoilerPlate.Application.Authentication.Common;
using MyApiBoilerPlate.Application.Authentication.Queries.Login;
using MyApiBoilerPlate.Requests.Users;
using MyApiBoilerPlate.Requests.Authentication;

namespace MyApiBoilerPlate.API.Controllers
{
  [Route("auth")]
  [AllowAnonymous]
  public class AuthenticationController(ISender mediator, IMapper mapper) : ApiController
  {
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest request, CancellationToken cancellationToken)
    {
      RegisterCommand command = mapper.Map<RegisterCommand>(request);
      ErrorOr<AuthenticationResult> authResult = await mediator.Send(command, cancellationToken);

      return authResult.Match(Ok, Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
      LoginQuery query = new LoginQuery(request.Email, request.Password);
      ErrorOr<AuthenticationResult> authResult = await mediator.Send(query, cancellationToken);

      return authResult.Match(Ok, Problem);
    }
  }
}
