using MyApiBoilerPlate.Application.Common.Models;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
        Task<User?> GetUserById(int userId, CancellationToken cancellationToken);
        Task<PagedResult<User>> GetAllUsers(int page, int pageSize, string? sortBy, bool sortDescending, CancellationToken cancellationToken);
        Task UpdateUser(User user, CancellationToken cancellationToken);
        Task DeleteUser(int userId, CancellationToken cancellationToken);
        Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken);
    }
}