using Microsoft.Data.SqlClient;

namespace MyApiBoilerPlate.Application.Common.Interfaces.Persistence
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}