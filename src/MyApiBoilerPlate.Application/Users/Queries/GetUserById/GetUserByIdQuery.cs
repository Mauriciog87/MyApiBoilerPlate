using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(int UserId) : IRequest<ErrorOr<User>>;
}