using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyApiBoilerPlate.Application.Common.Interfaces.Persistence;

namespace MyApiBoilerPlate.Infrastructure.Persistence
{
    internal sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
    {
        private readonly IConfiguration _configuration = configuration;

        public SqlConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("ConnectionString");

            return new SqlConnection(connectionString);
        }
    }
}