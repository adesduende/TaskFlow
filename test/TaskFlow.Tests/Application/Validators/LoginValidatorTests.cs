using TaskFlow.Application.UseCases.UserCases.Login;
using FluentValidation.TestHelper;

namespace TaskFlow.Tests.Application.Validators
{
    public class LoginValidatorTests
    {
        private readonly LoginValidator _validator;

        public LoginValidatorTests()
        {
            _validator = new LoginValidator();
        }

        [Fact]
        public void LoginValidator_ShouldHaveError_WhenEmailIsEmpty()
        {
            // Arrange
            var command = new LoginCommand("", "password123");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.email)
                  .WithErrorMessage("Email is required.");
        }

        [Fact]
        public void LoginValidator_ShouldHaveError_WhenEmailIsInvalid()
        {
            // Arrange
            var command = new LoginCommand("invalid-email", "password123");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.email)
                  .WithErrorMessage("Invalid email format.");
        }

        [Fact]
        public void LoginValidator_ShouldHaveError_WhenPasswordIsEmpty()
        {
            // Arrange
            var command = new LoginCommand("test@example.com", "");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.password)
                  .WithErrorMessage("Password is required.");
        }

        [Fact]
        public void LoginValidator_ShouldNotHaveError_WhenDataIsValid()
        {
            // Arrange
            var command = new LoginCommand("test@example.com", "password123");

            // Act & Assert
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}