using TaskFlow.Application.UseCases.GroupCases.GetGroupById;
using TaskFlow.Application.UseCases.GroupCases.CreateGroup;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.Group
{
    public class GetGroupByIdHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly Guid _ownerId;
        private readonly Guid _groupId;

        public GetGroupByIdHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            
            // Create test user and group
            var ownerResult = _mediator.SendAsync(new CreateUserCommand("Group Owner", "getgroupowner@example.com", "password123")).GetAwaiter().GetResult();
            _ownerId = ownerResult;

            var groupResult = _mediator.SendAsync(new CreateGroupCommand(
                "Test Group",
                "Test Group Description",
                new List<Guid>(),
                new List<Guid>(),
                _ownerId
            )).GetAwaiter().GetResult();
            _groupId = groupResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task GetGroupById_ShouldReturnGroup_WhenGroupExists()
        {
            // Arrange
            var query = new GetGroupByIdQuery(_groupId);

            // Act
            var result = await _mediator.SendAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_groupId.ToString(), result.Id);
            Assert.Equal("Test Group", result.Name);
            Assert.Equal("Test Group Description", result.Description);
            Assert.Equal(_ownerId.ToString(), result.OwnerId);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetGroupById_ShouldThrowKeyNotFoundException_WhenGroupDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var query = new GetGroupByIdQuery(nonExistentId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(query));
        }
    }
}