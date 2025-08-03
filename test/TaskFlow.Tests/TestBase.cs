using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application;
using TaskFlow.Infrastructure;
using TaskFlow.Domain.Repositories;
using Moq;
using TaskFlow.Infrastructure.HashPassword;

namespace TaskFlow.Tests.Application
{
    public class TestBase : IDisposable
    {
        protected readonly ServiceProvider ServiceProvider;
        protected readonly IMediator Mediator;
        protected readonly IPasswordHasher PasswordHasher;
        protected readonly IJwtTokenGenerator JwtTokenGenerator;

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

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            ServiceProvider?.Dispose();
        }
    }
}