using ErrorOr;
using MapsterMapper;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using MyApiBoilerPlate.Application.Users.Commands.CreateUser;
using MyApiBoilerPlate.Application.Users.Commands.DeleteUser;
using MyApiBoilerPlate.Application.Users.Commands.UpdateUser;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Application.Users.Queries.GetUserById;
using MyApiBoilerPlate.Domain.Entities;
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

        [HttpGet("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int userId)
        {
            GetUserByIdQuery query = new(userId);
            ErrorOr<User> result = await mediator.Send(query);

            return result.Match(Ok, Problem);
        }

        [HttpPut("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int userId, [FromBody] UpdateUserRequest request)
        {
            UpdateUserCommand command = new(
                userId,
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.DateOfBirth,
                request.IsActive
            );

            ErrorOr<bool> result = await mediator.Send(command);

            return result.Match(_ => NoContent(), Problem);
        }

        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int userId)
        {
            DeleteUserCommand command = new(userId);
            ErrorOr<bool> result = await mediator.Send(command);

            return result.Match(_ => NoContent(), Problem);
        }
    }
}