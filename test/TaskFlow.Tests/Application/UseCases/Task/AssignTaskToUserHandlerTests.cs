using TaskFlow.Application.UseCases.TaskCases.AssignTaskToUser;
using TaskFlow.Application.UseCases.TaskCases.CreateTask;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.Task
{
    public class AssignTaskToUserHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly ITaskRepository _taskRepository;
        private readonly Guid _userId1;
        private readonly Guid _userId2;

        public AssignTaskToUserHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            _taskRepository = ServiceProvider.GetRequiredService<ITaskRepository>();
            
            // Create test users
            var user1Result = _mediator.SendAsync(new CreateUserCommand("Assign User 1", "assignuser1@example.com", "password123")).GetAwaiter().GetResult();
            _userId1 = user1Result;

            var user2Result = _mediator.SendAsync(new CreateUserCommand("Assign User 2", "assignuser2@example.com", "password123")).GetAwaiter().GetResult();
            _userId2 = user2Result;
        }

        [Fact]
        public async System.Threading.Tasks.Task AssignTaskToUser_ShouldReturnTrue_WhenTaskAndUserExist()
        {
            // Cambia el orden de los argumentos en la creación de CreateTaskCommand para que coincida con la firma:
            // title, description, priority, timeLimit, userId, status, groupId

            var taskId = await _mediator.SendAsync(new CreateTaskCommand(
                "Assign Task",
                "Description",
                PriorityEnum.Medium,
                DateTime.UtcNow.AddDays(7), 
                _userId1,                   
                StatusEnum.NotStarted,      
                null                        
            ));

            var command = new AssignTaskToUserCommand(taskId, _userId2);

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.True(result);
            
            // Verify assignment
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            Assert.Equal(_userId2, task!.UserId);
        }

        [Fact]
        public async System.Threading.Tasks.Task AssignTaskToUser_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var nonExistentTaskId = Guid.NewGuid();
            var command = new AssignTaskToUserCommand(nonExistentTaskId, _userId1);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _mediator.SendAsync(command));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public async System.Threading.Tasks.Task AssignTaskToUser_ShouldThrowArgumentException_WhenIdsAreEmpty(string emptyGuid)
        {
            // Arrange
            var taskId = await _mediator.SendAsync(new CreateTaskCommand(
                "Assign Task Empty",
                "Description",
                PriorityEnum.Medium,
                DateTime.UtcNow.AddDays(7),
                _userId1,
                StatusEnum.NotStarted,
                null
            ));

            var command = new AssignTaskToUserCommand(taskId, Guid.Parse(emptyGuid));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _mediator.SendAsync(command));
        }
    }
}