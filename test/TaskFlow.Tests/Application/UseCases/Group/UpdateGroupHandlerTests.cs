using TaskFlow.Application.UseCases.GroupCases.UpdateGroups;
using TaskFlow.Application.UseCases.GroupCases.CreateGroup;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;
using FluentValidation;

namespace TaskFlow.Tests.Application.UseCases.Group
{
    public class UpdateGroupHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateGroupCommand> _validator;
        private readonly IGroupRepository _groupRepository;
        private readonly Guid _ownerId;

        public UpdateGroupHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            _groupRepository = ServiceProvider.GetRequiredService<IGroupRepository>();
            _validator = ServiceProvider.GetRequiredService<IValidator<UpdateGroupCommand>>();

            // Create test user
            var ownerResult = _mediator.SendAsync(new CreateUserCommand("Update Owner", "updateowner@example.com", "password123")).GetAwaiter().GetResult();
            _ownerId = ownerResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateGroup_ShouldReturnGroupId_WhenGroupExistsAndDataIsValid()
        {
            // Arrange
            var groupId = await _mediator.SendAsync(new CreateGroupCommand(
                "Original Group",
                "Original Description",
                new List<Guid>(),
                new List<Guid>(),
                _ownerId
            ));

            var command = new UpdateGroupCommand(
                groupId,
                "Updated Group",
                "Updated Description",
                new List<Guid>(),
                new List<Guid>()
            );

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.Equal(groupId, result);
            
            // Verify update
            var group = await _groupRepository.GetByIdAsync(groupId);
            Assert.Equal("Updated Group", group.Name);
            Assert.Equal("Updated Description", group.Description);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateGroup_ShouldThrowKeyNotFoundException_WhenGroupDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var command = new UpdateGroupCommand(
                nonExistentId,
                "Updated Group",
                "Updated Description",
                new List<Guid>(),
                new List<Guid>()
            );

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(command));
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateGroup_ShouldThrowAnException_WhenCommandIsNull()
        {
            // Arrange
            UpdateGroupCommand command = null;

            // Act & Assert
            await Assert.ThrowsAsync<System.InvalidOperationException>(
                async () => await _validator.ValidateAndThrowAsync(command));
        }
    }
}