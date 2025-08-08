using TaskFlow.Application.UseCases.TaskCases.GetTaskById;
using TaskFlow.Application.UseCases.TaskCases.CreateTask;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.Task
{
    public class GetTaskByIdHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly Guid _userId;
        private readonly Guid _taskId;

        public GetTaskByIdHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            
            // Create test user and task
            var userResult = _mediator.SendAsync(new CreateUserCommand("Task User", "gettaskuser@example.com", "password123")).GetAwaiter().GetResult();
            _userId = userResult;

            var taskResult = _mediator.SendAsync(new CreateTaskCommand(
                "Test Task",
                "Test Description",
                PriorityEnum.Medium,
                DateTime.UtcNow.AddDays(7),
                _userId,
                StatusEnum.NotStarted,
                null
            )).GetAwaiter().GetResult();
            _taskId = taskResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskById_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var query = new GetTaskByIdQuery(_taskId);

            // Act
            var result = await _mediator.SendAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_taskId.ToString(), result.Id);
            Assert.Equal("Test Task", result.Title);
            Assert.Equal("Test Description", result.Description);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskById_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var query = new GetTaskByIdQuery(nonExistentId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(query));
        }
    }
}