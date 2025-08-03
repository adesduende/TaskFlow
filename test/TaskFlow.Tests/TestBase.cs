using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application;
using TaskFlow.Infrastructure;
using TaskFlow.Domain.Repositories;
using Moq;

namespace TaskFlow.Tests.Application
{
    public class TestBase : IDisposable
    {
        protected readonly ServiceProvider ServiceProvider;
        protected readonly IMediator Mediator;

        public TestBase()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"JwtSettings:SecretKey", "your-test-secret-key-that-is-at-least-32-characters-long"},
                    {"JwtSettings:Issuer", "test-issuer"},
                    {"JwtSettings:Audience", "test-audience"},
                    {"ConnectionStrings:DefaultConnection", "Data Source=:memory:"}
                })
                .Build();

            // Registrar servicios de aplicación e infraestructura
            services.AddApplication();
            services.AddInfrastructure(configuration);

            // Configurar mocks para servicios externos si es necesario
            ConfigureMocks(services);

            ServiceProvider = services.BuildServiceProvider();

            Mediator = ServiceProvider.GetRequiredService<IMediator>();
        }

        protected virtual void ConfigureMocks(IServiceCollection services)
        {
            services.AddScoped<IUserRepository>(_ => Mock.Of<IUserRepository>());
            services.AddScoped<ITaskRepository>(_ => Mock.Of<ITaskRepository>());
            services.AddScoped<IGroupRepository>(_ => Mock.Of<IGroupRepository>());
        }

        public void Dispose()
        {
            ServiceProvider?.Dispose();
        }
    }
}