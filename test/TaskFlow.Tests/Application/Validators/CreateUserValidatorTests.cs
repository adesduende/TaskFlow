using TaskFlow.Application.UseCases.UserCases.CreateUser;
using FluentValidation.TestHelper;

namespace TaskFlow.Tests.Application.Validators
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator _validator;

        public CreateUserValidatorTests()
        {
            _validator = new CreateUserValidator();
        }

        [Fact]
        public void CreateUserValidator_ShouldHaveError_WhenNameIsEmpty()
        {
            // Arrange
            var command = new CreateUserCommand("", "test@example.com", "password123");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.name)
                  .WithErrorMessage("Name cannot be empty.");
        }

        [Fact]
        public void CreateUserValidator_ShouldHaveError_WhenNameExceedsMaxLength()
        {
            // Arrange
            var longName = new string('a', 101);
            var command = new CreateUserCommand(longName, "test@example.com", "password123");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.name)
                  .WithErrorMessage("Name cannot exceed 100 characters.");
        }

        [Fact]
        public void CreateUserValidator_ShouldHaveError_WhenEmailIsInvalid()
        {
            // Arrange
            var command = new CreateUserCommand("Test User", "invalid-email", "password123");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.email)
                  .WithErrorMessage("Invalid email format.");
        }

        [Fact]
        public void CreateUserValidator_ShouldHaveError_WhenPasswordIsTooShort()
        {
            // Arrange
            var command = new CreateUserCommand("Test User", "test@example.com", "12345");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.password)
                  .WithErrorMessage("Password must be at least 6 characters long.");
        }

        [Fact]
        public void CreateUserValidator_ShouldNotHaveError_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateUserCommand("Test User", "test@example.com", "password123");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}