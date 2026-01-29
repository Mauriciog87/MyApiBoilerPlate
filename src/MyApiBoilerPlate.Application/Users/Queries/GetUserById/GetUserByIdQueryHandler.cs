using ErrorOr;
using MapsterMapper;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Queries.GetUserById
{
  public sealed class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
      : IRequestHandler<GetUserByIdQuery, ErrorOr<UserResponse>>
  {
    public async ValueTask<ErrorOr<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
      User? user = await userRepository.GetUserById(request.UserId, cancellationToken);

      if (user is null)
      {
        return Application.Common.Errors.Errors.User.NotFoundById(request.UserId);
      }

      return mapper.Map<UserResponse>(user);
    }
  }
}
