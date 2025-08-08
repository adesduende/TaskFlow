using TaskFlow.Application.Mappers;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Tests.Application.Mappers
{
    public class UserMapperTests
    {
        [Fact]
        public void UserMapper_ToDto_ShouldMapCorrectly_WhenUserIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User(userId, "Test User", "test@example.com", "hashedpassword");

            // Act
            var dto = user.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(userId.ToString(), dto.Id);
            Assert.Equal("Test User", dto.Name);
            Assert.Equal("test@example.com", dto.Email);
        }

        [Fact]
        public void UserMapper_ToDto_ShouldThrowArgumentNullException_WhenUserIsNull()
        {
            // Arrange
            User user = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => user.ToDto());
        }
    }
}