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
            // Registering the TaskFlowMediator as the implementation of IMediator
            services.AddScoped<IMediator, TaskFlowMediator>();

            // Registering repositories with singleton lifetime
            services.AddSingleton<IUserRepository, UserInMemoryRepository>();
            services.AddSingleton<ITaskRepository, TaskInMemoryRepository>();
            services.AddSingleton<IGroupRepository, GroupInMemoryRepository>();

            return services;
        }
    }
}
