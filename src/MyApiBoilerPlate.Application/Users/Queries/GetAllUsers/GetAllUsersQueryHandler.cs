using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler(IUserRepository userRepository) 
        : IRequestHandler<GetAllUsersQuery, ErrorOr<Result<IEnumerable<User>>>>
    {
        public async ValueTask<ErrorOr<Result<IEnumerable<User>>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            Result<IEnumerable<User>> users = await userRepository
                .GetAllUsers(request.Page, request.PageSize, request.SortBy, request.SortDescending, cancellationToken);

            return users;
        }
    }
}