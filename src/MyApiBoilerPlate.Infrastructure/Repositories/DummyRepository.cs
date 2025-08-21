using Dapper;
using Microsoft.Data.SqlClient;
using MyApiBoilerPlate.Application.Common.Interfaces.Persistence;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using System.Data;

namespace MyApiBoilerPlate.Infrastructure.Repositories
{
    public sealed class DummyRepository(ISqlConnectionFactory connectionFactory) : IDummyRepository
    {
        public async Task AddDummyDataAsync(string data, CancellationToken cancellationToken = default)
        {
            DynamicParameters parameters = new();
            parameters.Add("Data", data, DbType.String, ParameterDirection.Input);

            using SqlConnection connection = connectionFactory.CreateConnection();

            _ = await connection.ExecuteAsync(
                "AddDummyData",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public Task DeleteDummyDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DummyDataExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetDummyDataAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDummyDataByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetDummyDataByPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDummyDataAsync(Guid id, string data, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
