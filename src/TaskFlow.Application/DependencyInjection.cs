using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
