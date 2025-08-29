using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery(
        int Page,
        int PageSize,
        string? SortBy,
        bool SortDescending) : IRequest<ErrorOr<Result<IEnumerable<User>>>>;
}