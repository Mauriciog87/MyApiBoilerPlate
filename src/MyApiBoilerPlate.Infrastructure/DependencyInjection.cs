using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApiBoilerPlate.Application.Common.Interfaces.Persistence;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Infrastructure.Persistence;
using MyApiBoilerPlate.Infrastructure.Repositories;
using MyApiBoilerPlate.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyApiBoilerPlate.Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;

namespace MyApiBoilerPlate.Infrastructure
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
      services.AddScoped<IDummyRepository, DummyRepository>();

      // Register UserRepository with logging decorator
      services.AddScoped<UserRepository>();
      services.AddScoped<IUserRepository>(provider =>
      {
        UserRepository innerRepository = provider.GetRequiredService<UserRepository>();
        ILogger<LoggingUserRepository> logger = provider.GetRequiredService<ILogger<LoggingUserRepository>>();
        return new LoggingUserRepository(innerRepository, logger);
      });

      services.AddAuthenticationInternal(configuration);

      return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services, IConfiguration configuration)
    {
      JwtSettings jwtSettings = new JwtSettings();
      configuration.Bind(JwtSettings.SectionName, jwtSettings);

      services.AddSingleton(Microsoft.Extensions.Options.Options.Create(jwtSettings));
      services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
      services.AddSingleton<IPasswordHasher, PasswordHasher>();

      services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(jwtSettings.Secret))
          });

      return services;
    }
  }
}