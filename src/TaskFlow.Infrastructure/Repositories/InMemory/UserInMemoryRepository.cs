using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Infrastructure.Repositories.InMemory
{
    public class UserInMemoryRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserInMemoryRepository()
        {
            _users = new List<User>();
        }
        public System.Threading.Tasks.Task AddUserAsync(User user)
        {
            _users.Add(user);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task DeleteUserAsync(Guid userId)
        {
            _users.RemoveAll(u => u.Id == userId);
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return System.Threading.Tasks.Task.FromResult<IEnumerable<User>>(_users);
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return System.Threading.Tasks.Task.FromResult(user);
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            return System.Threading.Tasks.Task.FromResult(user);
        }

        public System.Threading.Tasks.Task UpdateUserAsync(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.UpdateName(user.Name);
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public Task<bool> UserEmailExistsAsync(string email)
        {
            var exists = _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return System.Threading.Tasks.Task.FromResult(exists);
        }

        public Task<bool> UserExistsAsync(Guid userId)
        {
            var exists = _users.Any(u => u.Id == userId);
            return System.Threading.Tasks.Task.FromResult(exists);
        }
    }
}
