using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApiBoilerPlate.Application.Common.Interfaces.Persistence;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Infrastructure.Persistence;
using MyApiBoilerPlate.Infrastructure.Repositories;

namespace MyApiBoilerPlate.Infrastructure
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
      services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
      services.AddScoped<IDummyRepository, DummyRepository>();
      
      // Register UserRepository with logging decorator
      // UserRepository is registered as concrete type so DI container manages its lifetime
      services.AddScoped<UserRepository>();
      services.AddScoped<IUserRepository>(provider =>
      {
        var innerRepository = provider.GetRequiredService<UserRepository>();
        var logger = provider.GetRequiredService<ILogger<LoggingUserRepository>>();
        return new LoggingUserRepository(innerRepository, logger);
      });

      return services;
    }
  }
}