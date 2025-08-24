using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Users.Common;

namespace MyApiBoilerPlate.Application.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        DateTime DateOfBirth,
        bool IsActive
    ) : IRequest<ErrorOr<UserCreatedResult>>;
}