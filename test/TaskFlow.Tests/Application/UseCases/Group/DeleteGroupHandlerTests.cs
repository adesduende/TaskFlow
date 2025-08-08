using TaskFlow.Application.UseCases.GroupCases.DeleteGroup;
using TaskFlow.Application.UseCases.GroupCases.CreateGroup;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.Group
{
    public class DeleteGroupHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly IGroupRepository _groupRepository;
        private readonly Guid _ownerId;

        public DeleteGroupHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            _groupRepository = ServiceProvider.GetRequiredService<IGroupRepository>();
            
            // Create test user
            var ownerResult = _mediator.SendAsync(new CreateUserCommand("Delete Owner", "deleteowner@example.com", "password123")).GetAwaiter().GetResult();
            _ownerId = ownerResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteGroup_ShouldReturnTrue_WhenGroupExists()
        {
            // Arrange
            var groupId = await _mediator.SendAsync(new CreateGroupCommand(
                "Group to Delete",
                "Description",
                new List<Guid>(),
                new List<Guid>(),
                _ownerId
            ));

            var command = new DeleteGroupCommand(groupId);

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.True(result);
            
            // Verify deletion
            var group = await _groupRepository.GetByIdAsync(groupId);
            Assert.Null(group);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteGroup_ShouldThrowKeyNotFoundException_WhenGroupDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var command = new DeleteGroupCommand(nonExistentId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(command));
        }
    }
}