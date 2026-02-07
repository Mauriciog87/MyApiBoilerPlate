namespace MyApiBoilerPlate.Application.Common.Interfaces.Persistence
{
  public interface ISqlConnectionFactory
  {
    Task<System.Data.IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
  }
}