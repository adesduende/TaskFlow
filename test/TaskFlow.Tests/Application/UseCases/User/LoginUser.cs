using TaskFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.UseCases.UserCases.Login;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.UseCases.UserCases.CreateUser;

namespace TaskFlow.Tests.Application.UseCases.User
{
    public class LoginUser : TestBase
    {
        private readonly string _userId;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public LoginUser() : base()
        {
            // Configurar el repositorio de usuarios
            _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            _mediator = ServiceProvider.GetRequiredService<IMediator>();

            // Configurar el usuario de prueba
            var result = _mediator.SendAsync(new CreateUserCommand("Sergio","test@test.com", "password123!")).GetAwaiter().GetResult();
            _userId = result.ToString();
        }

        [Fact]
        public async Task LoginUser_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@test.com";
            var password = "password123!";
            var command = new LoginCommand(email, password);
            
            // Act
            var result = await _mediator.SendAsync(command);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
            Assert.NotEmpty(result.UserId);
            Assert.Equal(_userId, result.UserId);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnFailure_WhenCredentialsAreInvalid()
        {
            // Arrange
            var email = "test@test.com";
            var password = "wrongpassword";
            var command = new LoginCommand(email, password);
            
            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _mediator.SendAsync(command));
        }
    }
}
