using ErrorOr;
using Mediator;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandHandler(
        IUserRepository userRepository
    ) : IRequestHandler<UpdateUserCommand, ErrorOr<bool>>
    {
        public async ValueTask<ErrorOr<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User? existingUser = await userRepository.GetUserById(request.UserId, cancellationToken);
            
            if (existingUser is null)
            {
                return Application.Common.Errors.Errors.User.NotFoundById(request.UserId);
            }

            // Check if email is being changed and if new email already exists
            if (!string.Equals(existingUser.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                bool emailExists = await userRepository.CheckIfUserExists(request.Email, cancellationToken);
                if (emailExists)
                {
                    return Application.Common.Errors.Errors.User.AlreadyExistsByEmail(request.Email);
                }
            }

            // Update the existing user using domain method
            existingUser.Update(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.DateOfBirth,
                request.IsActive
            );

            await userRepository.UpdateUser(existingUser, cancellationToken);
            return true;
        }
    }
}
