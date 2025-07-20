using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.Services
{
    internal class UserService
    {
        public IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Create a new user with the specified parameters.
        /// </summary>
        /// <param name="name"> The name of the user.</param>
        /// <param name="email"> The email of the user.</param>
        /// <param name="password"> The password of the user.</param>
        /// <returns> A task representing the asynchronous operation, containing the ID of the created user.</returns>
        /// <exception cref="ArgumentException"> Thrown when any of the parameters are empty or null.</exception>
        public async Task<Guid> CreateUserAsync(
            string name,
            string email,
            string password
        )
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));
            var user = new Domain.Entities.User(Guid.NewGuid(), name, email);
            await _userRepository.AddUserAsync(user);
            return user.Id;
        }

        /// <summary>
        /// Get a user by their ID.
        /// </summary>
        /// <param name="userId"> The ID of the user to retrieve.</param>
        /// <returns> A task representing the asynchronous operation, containing the user DTO.</returns>
        /// <exception cref="ArgumentException"> Thrown when the user ID is empty.</exception>
        /// <exception cref="KeyNotFoundException"> Thrown when the user with the specified ID does not exist.</exception>
        public async Task<UserDTO> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return Mappers.UserMapper.ToDto(user);
        }
    }
}
