using ErrorOr;
using MapsterMapper;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Application.Common.Models;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Queries.GetAllUsers
{
    public sealed class GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        : IRequestHandler<GetAllUsersQuery, ErrorOr<PagedResult<UserResponse>>>
    {
        public async ValueTask<ErrorOr<PagedResult<UserResponse>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            PagedResult<User> users = await userRepository
                .GetAllUsers(request.Page, request.PageSize, request.SortBy, request.SortDescending, cancellationToken);

            IEnumerable<UserResponse> userResponses = users.Data.Select(mapper.Map<UserResponse>);

            return new PagedResult<UserResponse>(
                userResponses,
                users.PageSize,
                users.PageNumber,
                users.TotalRecords,
                users.OrderDescending,
                users.OrderBy
            );
        }
    }
}
