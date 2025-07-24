using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure.Mediator;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, TaskFlowMediator>();
            return services;
        }
    }
}
