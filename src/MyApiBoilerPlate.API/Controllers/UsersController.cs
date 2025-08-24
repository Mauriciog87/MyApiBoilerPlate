using ErrorOr;
using MapsterMapper;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using MyApiBoilerPlate.Application.Users.Commands.CreateUser;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Requests.Users;

namespace MyApiBoilerPlate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator, IMapper mapper) : ApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            CreateUserCommand command = mapper.Map<CreateUserCommand>(request);
            ErrorOr<UserCreatedResult> result = await mediator.Send(command);

            return result.Match(Ok, Problem);
        }
    }
}