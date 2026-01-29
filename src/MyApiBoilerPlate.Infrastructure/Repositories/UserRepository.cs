using Dapper;
using MyApiBoilerPlate.Application.Common.Interfaces.Persistence;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Application.Common.Models;
using MyApiBoilerPlate.Domain.Entities;
using System.Data;

namespace MyApiBoilerPlate.Infrastructure.Repositories
{
    public class UserRepository(ISqlConnectionFactory connectionFactory) : IUserRepository
    {
        public async Task<User> CreateUser(User user, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("FirstName", user.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("LastName", user.LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("Email", user.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("PasswordHash", user.PasswordHash, DbType.String, ParameterDirection.Input);
            parameters.Add("PhoneNumber", user.PhoneNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("DateOfBirth", user.DateOfBirth, DbType.Date, ParameterDirection.Input);
            parameters.Add("IsActive", user.IsActive, DbType.Boolean, ParameterDirection.Input);

            using IDbConnection connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleAsync<User>(
                "sp_InsertUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteUser(int userId, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("UserId", userId, DbType.Int32, ParameterDirection.Input);

            using IDbConnection connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            await connection.ExecuteAsync(
                "sp_DeleteUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<PagedResult<User>> GetAllUsers(
            int pageNumber,
            int pageSize,
            string? sortBy,
            bool sortDescending,
            CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("PageNumber", pageNumber, DbType.Int32, ParameterDirection.Input);
            parameters.Add("PageSize", pageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("SortBy", sortBy, DbType.String, ParameterDirection.Input);
            parameters.Add("SortDescending", sortDescending, DbType.Boolean, ParameterDirection.Input);

            using IDbConnection connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            SqlMapper.GridReader query = await connection.QueryMultipleAsync(
                "sp_GetAllUsersPaginated",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int totalRecords = await query.ReadFirstOrDefaultAsync<int>();

            IEnumerable<User> users = await query.ReadAsync<User>();

            return new PagedResult<User>(users, pageSize, pageNumber, totalRecords, sortDescending, sortBy);
        }

        public async Task<User?> GetUserById(int userId, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("UserId", userId, DbType.Int32, ParameterDirection.Input);

            using IDbConnection connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<User>(
                "sp_GetUserById",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateUser(User user, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("UserId", user.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("FirstName", user.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("LastName", user.LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("Email", user.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("PhoneNumber", user.PhoneNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("DateOfBirth", user.DateOfBirth, DbType.Date, ParameterDirection.Input);
            parameters.Add("IsActive", user.IsActive, DbType.Boolean, ParameterDirection.Input);

            using IDbConnection connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            await connection.ExecuteAsync(
                "sp_UpdateUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("Email", email, DbType.String, ParameterDirection.Input);

            using IDbConnection connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<User>(
                "sp_GetUserByEmail",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new();
            parameters.Add("Email", email, DbType.String, ParameterDirection.Input);

            using IDbConnection connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);

            return await connection.QuerySingleAsync<bool>(
                "sp_CheckEmailExists",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}