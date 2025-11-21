using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Common.Models;
using MyApiBoilerPlate.Application.Users.Common;

namespace MyApiBoilerPlate.Application.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery(
        int Page,
        int PageSize,
        string? SortBy,
        bool SortDescending) : IRequest<ErrorOr<PagedResult<UserResponse>>>;
}