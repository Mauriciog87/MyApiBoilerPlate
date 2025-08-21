using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using MyApiBoilerPlate.Application.Common.Behaviors;
using MyApiBoilerPlate.Application.Dummy.Queries.Test;
using System.Reflection;

namespace MyApiBoilerPlate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator((MediatorOptions options) =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.Assemblies = [typeof(TestQuery)];
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}