using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Users.Common;

namespace MyApiBoilerPlate.Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(int UserId) : IRequest<ErrorOr<UserResponse>>;
}