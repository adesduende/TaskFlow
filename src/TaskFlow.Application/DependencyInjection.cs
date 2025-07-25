using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.UseCases.TaskCases.CreateTask;

namespace TaskFlow.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register all IMediat handlers in the assembly
            services.Scan(
                scan => scan
                    .FromAssemblyOf<CreateTaskHandler>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );

            // Register all INotification handlers in the assembly
            services.AddValidatorsFromAssembly(typeof(CreateTaskHandler).Assembly);
            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}
