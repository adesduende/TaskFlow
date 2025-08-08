using TaskFlow.Application.UseCases.TaskCases.ChangeTaskStatus;
using TaskFlow.Application.UseCases.TaskCases.CreateTask;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.Task
{
    public class ChangeTaskStatusHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;
        private readonly Guid _userId;

        public ChangeTaskStatusHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            _taskRepository = ServiceProvider.GetRequiredService<ITaskRepository>();
            
            // Create test user
            var userResult = _mediator.SendAsync(new CreateUserCommand("Status User", "statususer@example.com", "password123")).GetAwaiter().GetResult();
            _userId = userResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task ChangeTaskStatus_ShouldReturnTrue_WhenTaskExistsAndStatusIsValid()
        {
            // Arrange
            var taskId = await _mediator.SendAsync(new CreateTaskCommand(
                "Status Task",
                "Description for status change",
                PriorityEnum.Medium,
                DateTime.UtcNow.AddDays(7),
                _userId,
                StatusEnum.NotStarted,
                null
            ));

            var command = new ChangeTaskStatusCommand(taskId, StatusEnum.InProgress);

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.True(result);
            
            // Verify status was changed
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            Assert.Equal(StatusEnum.InProgress, task!.Status);
        }

        [Fact]
        public async System.Threading.Tasks.Task ChangeTaskStatus_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var command = new ChangeTaskStatusCommand(nonExistentId, StatusEnum.Completed);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(command));
        }

        [Fact]
        public async System.Threading.Tasks.Task ChangeTaskStatus_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var command = new ChangeTaskStatusCommand(Guid.Empty, StatusEnum.Completed);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _mediator.SendAsync(command));
        }
    }
}