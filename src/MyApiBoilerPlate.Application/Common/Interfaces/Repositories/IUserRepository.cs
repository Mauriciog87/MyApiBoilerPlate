using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
        Task<User?> GetUserById(int userId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<User>>> GetAllUsers(int page, int pageSize, string? sortyBy, bool sortDescending, CancellationToken cancellationToken);
        Task UpdateUser(User user, CancellationToken cancellationToken);
        Task DeleteUser(int userId, CancellationToken cancellationToken);
        Task<User> GetUserByGuid(Guid id);
        Task<User?> GetUserByEmail(string email);
        Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken);
    }
}