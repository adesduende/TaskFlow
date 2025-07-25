using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;
using TaskFlow.Infrastructure.Mediator;
using TaskFlow.Infrastructure.Repositories.InMemory;

namespace TaskFlow.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IMediator, TaskFlowMediator>();

            services.AddSingleton<IUserRepository, UserInMemoryRepository>();
            services.AddSingleton<ITaskRepository, TaskInMemoryRepository>();
            return services;
        }
    }
}
