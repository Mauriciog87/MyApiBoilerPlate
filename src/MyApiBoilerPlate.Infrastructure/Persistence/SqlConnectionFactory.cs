using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyApiBoilerPlate.Application.Common.Interfaces.Persistence;

namespace MyApiBoilerPlate.Infrastructure.Persistence
{
  internal sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
  {
    private readonly IConfiguration _configuration = configuration;

    public async Task<System.Data.IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
      var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString"));
      await connection.OpenAsync(cancellationToken);
      return connection;
    }
  }
}
