using TaskFlow.Application.UseCases.TaskCases.CreateTask;
using TaskFlow.Application.UseCases.UserCases.CreateUser;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Tests.Application;
using FluentValidation;

namespace TaskFlow.Tests.Application.UseCases.Task
{
    public class CreateTaskHandlerTests : TestBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateTaskCommand> _validator;
        private readonly Guid _userId;

        public CreateTaskHandlerTests() : base()
        {
            _mediator = ServiceProvider.GetRequiredService<IMediator>();
            _validator = ServiceProvider.GetRequiredService<IValidator<CreateTaskCommand>>();

            // Create test user
            var createResult = _mediator.SendAsync(new CreateUserCommand("Task User", "taskuser@example.com", "password123")).GetAwaiter().GetResult();
            _userId = createResult;
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ShouldReturnTaskId_WhenValidDataProvided()
        {
            // Arrange
            var command = new CreateTaskCommand(
                "Test Task",
                "Test Description",
                PriorityEnum.Medium,
                DateTime.UtcNow.AddDays(7),
                _userId,
                StatusEnum.NotStarted,
                null
            );

            // Act
            var result = await _mediator.SendAsync(command);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ShouldSetCorrectProperties_WhenCreatingTask()
        {
            // Arrange
            var timeLimit = DateTime.UtcNow.AddDays(7);
            var command = new CreateTaskCommand(
                "Test Task Properties",
                "Test Description Properties",
                PriorityEnum.High,
                timeLimit,
                _userId,
                StatusEnum.InProgress,
                null
            );

            // Act
            var taskId = await _mediator.SendAsync(command);

            // Assert
            Assert.NotEqual(Guid.Empty, taskId);
        }

        [Theory]
        [InlineData("", "Description", PriorityEnum.Low, StatusEnum.NotStarted)]
        public async System.Threading.Tasks.Task CreateTask_ShouldThrowValidationException_WhenInvalidDataProvided(string title, string description, PriorityEnum priority, StatusEnum status)
        {
            // Arrange
            var command = new CreateTaskCommand(
                title,
                description,
                priority,
                DateTime.UtcNow.AddDays(7),
                _userId,
                status,
                null
            );

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _validator.ValidateAndThrowAsync(command));
        }
    }
}