using Mediator;
using MyApiBoilerPlate.API.Mapping;
using MyApiBoilerPlate.API.Pipelines;

namespace MyApiBoilerPlate.API
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
      services.AddControllers();
      services.AddMediator(options =>
      {
        options.ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped;
      });
      services.AddMappings();

      return services;
    }

    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
      services.AddExceptionHandler<GlobalExceptionHandler>();
      services.AddProblemDetails();

      return services;
    }
  }
}
