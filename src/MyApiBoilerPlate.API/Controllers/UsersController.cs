using ErrorOr;
using MapsterMapper;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using MyApiBoilerPlate.Application.Common.Models;
using MyApiBoilerPlate.Application.Users.Commands.CreateUser;
using MyApiBoilerPlate.Application.Users.Commands.DeleteUser;
using MyApiBoilerPlate.Application.Users.Commands.UpdateUser;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Application.Users.Queries.GetUserById;
using MyApiBoilerPlate.Application.Users.Queries.GetAllUsers;
using MyApiBoilerPlate.Requests.Users;

namespace MyApiBoilerPlate.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public sealed class UsersController(IMediator mediator, IMapper mapper) : ApiController
  {
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
      CreateUserCommand command = mapper.Map<CreateUserCommand>(request);
      ErrorOr<UserCreatedResult> result = await mediator.Send(command, cancellationToken);

      return result.Match(
          created => CreatedAtAction(nameof(GetById), new { userId = created.UserId }, created),
          Problem);
    }

    [HttpGet("{userId:int}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int userId, CancellationToken cancellationToken)
    {
      GetUserByIdQuery query = new(userId);
      ErrorOr<UserResponse> result = await mediator.Send(query, cancellationToken);

      return result.Match(Ok, Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
      GetAllUsersQuery query = new(page, pageSize, sortBy, sortDescending);
      ErrorOr<PagedResult<UserResponse>> result = await mediator.Send(query, cancellationToken);

      return result.Match(Ok, Problem);
    }

    [HttpPut("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        int userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
      UpdateUserCommand command = mapper.Map<UpdateUserCommand>((request, userId));

      ErrorOr<bool> result = await mediator.Send(command, cancellationToken);

      return result.Match(_ => NoContent(), Problem);
    }

    [HttpDelete("{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int userId, CancellationToken cancellationToken)
    {
      DeleteUserCommand command = new(userId);
      ErrorOr<bool> result = await mediator.Send(command, cancellationToken);

      return result.Match(_ => NoContent(), Problem);
    }
  }
}