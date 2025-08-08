using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;
using FluentValidation;

namespace TaskFlow.Tests.Application.UseCases.User
{
    public class CreateUserHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator _validator;

        public CreateUserHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateUser_ShouldReturnUserId_WhenValidDataProvided()
        {
            // Arrange
            var command = new CreateUserCommand("Test User", "test@example.com", "password123");

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateUser_ShouldHashPassword_WhenCreatingUser()
        {
            // Arrange
            var command = new CreateUserCommand("Test User", "test2@example.com", "password123");
            var userRepository = ServiceProvider.GetRequiredService<IUserRepository>();

            // Act
            var userId = await _mediator.SendAsync(command);
            var user = await userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(user);
            Assert.NotEqual("password123", user.Password); // Password should be hashed
        }

        [Theory]
        [InlineData("", "test@example.com", "password123")]
        [InlineData("Test User", "", "password123")]
        [InlineData("Test User", "test@example.com", "")]
        public async System.Threading.Tasks.Task CreateUser_ShouldThrowArgumentException_WhenInvalidDataProvided(string name, string email, string password)
        {
            // Arrange
            //Get validator
            var validator = ServiceProvider.GetRequiredService<IValidator<CreateUserCommand>>();
            var command = new CreateUserCommand(name, email, password);


            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await validator.ValidateAndThrowAsync(command));
        }
    }
}