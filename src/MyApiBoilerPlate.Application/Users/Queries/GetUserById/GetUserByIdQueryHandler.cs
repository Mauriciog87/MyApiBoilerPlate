using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUserRepository userRepository) 
        : IRequestHandler<GetUserByIdQuery, ErrorOr<User>>
    {
        public async ValueTask<ErrorOr<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetUserById(request.UserId, cancellationToken);

            if (user is null)
            {
                return Application.Common.Errors.Errors.User.NotFound;
            }

            return user;
        }
    }
}