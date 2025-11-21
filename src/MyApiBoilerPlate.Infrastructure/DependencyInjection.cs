using Microsoft.Extensions.DependencyInjection;
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
      services.AddScoped<IUserRepository, UserRepository>();

      return services;
    }
  }
}