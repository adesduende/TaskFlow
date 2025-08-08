using TaskFlow.Application.UseCases.GroupCases.CreateGroup;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;
using FluentValidation;

namespace TaskFlow.Tests.Application.UseCases.Group
{
    public class CreateGroupHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateGroupCommand> _validator;
        private readonly Guid _ownerId;

        public CreateGroupHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            _validator = ServiceProvider.GetRequiredService<IValidator<CreateGroupCommand>>();

            // Create test user to be group owner
            var ownerResult = _mediator.SendAsync(new CreateUserCommand("Group Owner", "groupowner@example.com", "password123")).GetAwaiter().GetResult();
            _ownerId = ownerResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateGroup_ShouldReturnGroupId_WhenValidDataProvided()
        {
            // Arrange
            var command = new CreateGroupCommand(
                "Test Group",
                "Test Group Description",
                new List<Guid>(),
                new List<Guid>(),
                _ownerId
            );

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateGroup_ShouldThrowValidationException_WhenNameIsEmpty()
        {
            // Arrange
            var command = new CreateGroupCommand(
                "",
                "Test Group Description",
                new List<Guid>(),
                new List<Guid>(),
                _ownerId
            );

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                async ()=> await _validator.ValidateAndThrowAsync(command));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateGroup_ShouldThrowValidationException_WhenOwnerIdIsEmpty()
        {
            // Arrange
            var command = new CreateGroupCommand(
                "Test Group",
                "Test Group Description",
                new List<Guid>(),
                new List<Guid>(),
                Guid.Empty
            );

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                async () => await _validator.ValidateAndThrowAsync(command));
        }
    }
}