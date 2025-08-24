using ErrorOr;
using MapsterMapper;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        IMapper mapper,
        IUserRepository userRepository
    ) : IRequestHandler<UpdateUserCommand, ErrorOr<bool>>
    {
        public async ValueTask<ErrorOr<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User? existingUser = await userRepository.GetUserById(request.UserId, cancellationToken);
            
            if (existingUser is null)
            {
                return Application.Common.Errors.Errors.User.NotFound;
            }

            // Check if email is being changed and if new email already exists
            if (!string.Equals(existingUser.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                bool emailExists = await userRepository.CheckIfUserExists(request.Email, cancellationToken);
                if (emailExists)
                {
                    return Application.Common.Errors.Errors.User.AlreadyExists;
                }
            }

            User userToUpdate = mapper.Map<User>(request);
            userToUpdate.Id = existingUser.Id;
            userToUpdate.UserId = existingUser.UserId;

            await userRepository.UpdateUser(userToUpdate, cancellationToken);
            return true;
        }
    }
}