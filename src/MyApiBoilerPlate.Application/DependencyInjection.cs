using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using MyApiBoilerPlate.Application.Common.Behaviors;
using System.Reflection;

namespace MyApiBoilerPlate.Application
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
      services.AddMediator(options =>
      {
        options.ServiceLifetime = ServiceLifetime.Scoped;
      });

      services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
      services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

      return services;
    }
  }
}