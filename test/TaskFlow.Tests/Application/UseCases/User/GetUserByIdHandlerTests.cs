using TaskFlow.Application.UseCases.UserCases.GetUserById;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.User
{
    public class GetUserByIdHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly Guid _userId;

        public GetUserByIdHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            
            // Create test user
            var createResult = _mediator.SendAsync(new CreateUserCommand("Test User", "gettest@example.com", "password123")).GetAwaiter().GetResult();
            _userId = createResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var query = new GetUserByIdQuery(_userId);

            // Act
            var result = await _mediator.SendAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_userId.ToString(), result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("gettest@example.com", result.Email);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUserById_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var query = new GetUserByIdQuery(nonExistentId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(query));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUserById_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var query = new GetUserByIdQuery(Guid.Empty);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _mediator.SendAsync(query));
        }
    }
}