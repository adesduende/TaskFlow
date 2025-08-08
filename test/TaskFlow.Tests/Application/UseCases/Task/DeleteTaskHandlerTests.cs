using TaskFlow.Application.UseCases.TaskCases.DeleteTask;
using TaskFlow.Application.UseCases.TaskCases.CreateTask;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.Task
{
    public class DeleteTaskHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;
        private readonly Guid _userId;

        public DeleteTaskHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            _taskRepository = ServiceProvider.GetRequiredService<ITaskRepository>();
            
            // Create test user
            var userResult = _mediator.SendAsync(new CreateUserCommand("Delete User", "deleteuser@example.com", "password123")).GetAwaiter().GetResult();
            _userId = userResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ShouldReturnTrue_WhenTaskExists()
        {
            // Arrange
            var taskId = await _mediator.SendAsync(new CreateTaskCommand(
                "Task to Delete",
                "Description",
                PriorityEnum.Medium,
                DateTime.UtcNow.AddDays(7),
                _userId,
                StatusEnum.NotStarted,
                null
            ));

            var command = new DeleteTaskCommand(taskId);

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.True(result);
            
            // Verify task no longer exists
            var taskExists = await _taskRepository.TaskExistsAsync(taskId);
            Assert.False(taskExists);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var command = new DeleteTaskCommand(nonExistentId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(command));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var command = new DeleteTaskCommand(Guid.Empty);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _mediator.SendAsync(command));
        }
    }
}