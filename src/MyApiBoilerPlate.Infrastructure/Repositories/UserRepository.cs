using Dapper;
using MyApiBoilerPlate.Application.Common.Interfaces.Persistence;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Infrastructure.Repositories
{
    public class UserRepository(ISqlConnectionFactory connectionFactory) : IUserRepository
    {
        public async Task<User> CreateUser(User user, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("Id", user.Id, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);
            parameters.Add("FirstName", user.FirstName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("LastName", user.LastName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("Email", user.Email, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("PhoneNumber", user.PhoneNumber, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("DateOfBirth", user.DateOfBirth, System.Data.DbType.Date, System.Data.ParameterDirection.Input);
            parameters.Add("IsActive", user.IsActive, System.Data.DbType.Boolean, System.Data.ParameterDirection.Input);

            using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleAsync<User>(
                "sp_InsertUser",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task DeleteUser(int userId, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("UserId", userId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            await connection.ExecuteAsync(
                "sp_DeleteUser",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<Result<IEnumerable<User>>> GetAllUsers(
            int pageNumber,
            int pageSize,
            string? sortyBy,
            bool sortDescending,
            CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("PageNumber", pageNumber, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("PageSize", pageSize, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("SortBy", sortyBy, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("SortDescending", sortDescending, System.Data.DbType.Boolean, System.Data.ParameterDirection.Input);

            using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            SqlMapper.GridReader query = await connection.QueryMultipleAsync(
                "sp_GetAllUsersPaginated",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );

            int totalRecords = await query.ReadFirstOrDefaultAsync<int>();

            IEnumerable<User> users = await query.ReadAsync<User>();

            return new Result<IEnumerable<User>>(users, pageSize, pageNumber, totalRecords, sortDescending, sortyBy);
        }

        public Task<User?> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByGuid(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserById(int userId, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("UserId", userId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

            using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<User>(
                "sp_GetUserById",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task UpdateUser(User user, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("UserId", user.UserId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("FirstName", user.FirstName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("LastName", user.LastName, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("Email", user.Email, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("PhoneNumber", user.PhoneNumber, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("DateOfBirth", user.DateOfBirth, System.Data.DbType.Date, System.Data.ParameterDirection.Input);
            parameters.Add("IsActive", user.IsActive, System.Data.DbType.Boolean, System.Data.ParameterDirection.Input);

            using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            await connection.ExecuteAsync(
                "sp_UpdateUser",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("Email", email, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleAsync<bool>(
                "sp_CheckEmailExists",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }
}