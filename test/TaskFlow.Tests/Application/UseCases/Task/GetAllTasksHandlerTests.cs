using TaskFlow.Application.UseCases.TaskCases.GetAllTasks;
using TaskFlow.Application.UseCases.TaskCases.CreateTask;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;

namespace TaskFlow.Tests.Application.UseCases.Task
{
    public class GetAllTasksHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly Guid _userId;

        public GetAllTasksHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            
            // Create test user
            var userResult = _mediator.SendAsync(new CreateUserCommand("Tasks User", "tasksuser@example.com", "password123")).GetAwaiter().GetResult();
            _userId = userResult;

            // Create test tasks
            _mediator.SendAsync(new CreateTaskCommand(
                "High Priority Task",
                "High Priority Description",
                PriorityEnum.High,
                DateTime.UtcNow.AddDays(7),
                _userId,
                StatusEnum.InProgress,
                null
            )).GetAwaiter().GetResult();

            _mediator.SendAsync(new CreateTaskCommand(
                "Low Priority Task",
                "Low Priority Description",
                PriorityEnum.Low,
                DateTime.UtcNow.AddDays(10),
                _userId,
                StatusEnum.NotStarted,
                null
            )).GetAwaiter().GetResult();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ShouldReturnAllTasks_WhenNoFiltersApplied()
        {
            // Arrange
            var query = new GetAllTasksQuery();

            // Act
            var result = await _mediator.SendAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ShouldFilterByPriority_WhenPriorityFilterApplied()
        {
            // Arrange
            var query = new GetAllTasksQuery(null, PriorityEnum.High, null, null, null);

            // Act
            var result = await _mediator.SendAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, task => Assert.Equal("High", task.Priority));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ShouldFilterByStatus_WhenStatusFilterApplied()
        {
            // Arrange
            var query = new GetAllTasksQuery(StatusEnum.InProgress, null, null, null, null);

            // Act
            var result = await _mediator.SendAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, task => Assert.Equal("InProgress", task.Status));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ShouldFilterByUser_WhenUserFilterApplied()
        {
            // Arrange
            var query = new GetAllTasksQuery(null, null, _userId, null, null);

            // Act
            var result = await _mediator.SendAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, task => Assert.Equal(_userId.ToString(), task.UserId));
        }
    }
}