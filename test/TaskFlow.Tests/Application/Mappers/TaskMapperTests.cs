using TaskFlow.Application.Mappers;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Tests.Application.Mappers
{
    public class TaskMapperTests
    {
        [Fact]
        public void TaskMapper_ToDto_ShouldMapCorrectly_WhenTaskIsValid()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;
            var timeLimit = DateTime.UtcNow.AddDays(7);

            var task = new Domain.Entities.Task(
                taskId,
                "Test Task",
                "Test Description",
                PriorityEnum.High,
                StatusEnum.InProgress,
                createdAt,
                timeLimit,
                userId,
                groupId
            );

            // Act
            var dto = task.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(taskId.ToString(), dto.Id);
            Assert.Equal("Test Task", dto.Title);
            Assert.Equal("Test Description", dto.Description);
            Assert.Equal("High", dto.Priority);
            Assert.Equal("InProgress", dto.Status);
            Assert.Equal(createdAt, dto.CreatedAt);
            Assert.Equal(timeLimit, dto.TimeLimit);
            Assert.Equal(userId.ToString(), dto.UserId);
            Assert.Equal(groupId.ToString(), dto.GroupId);
        }

        [Fact]
        public void TaskMapper_ToDto_ShouldHandleNullValues_WhenOptionalFieldsAreNull()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;

            var task = new Domain.Entities.Task(
                taskId,
                "Test Task",
                "Test Description",
                PriorityEnum.Low,
                StatusEnum.NotStarted,
                createdAt,
                null, // timeLimit
                userId,
                null  // groupId
            );

            // Act
            var dto = task.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(taskId.ToString(), dto.Id);
            Assert.Null(dto.TimeLimit);
            Assert.Equal(string.Empty, dto.GroupId);
        }

        [Fact]
        public void TaskMapper_ToDto_ShouldThrowArgumentNullException_WhenTaskIsNull()
        {
            // Arrange
            Domain.Entities.Task task = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => task.ToDto());
        }
    }
}