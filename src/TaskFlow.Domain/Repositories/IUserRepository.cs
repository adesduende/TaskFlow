using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Repositories
{
    public interface IUserRepository
    {
        System.Threading.Tasks.Task<IEnumerable<Entities.User>> GetAllUsersAsync();
        System.Threading.Tasks.Task<Entities.User?> GetUserByIdAsync(Guid userId);
        System.Threading.Tasks.Task<Entities.User?> GetUserByEmailAsync(string email);
        System.Threading.Tasks.Task AddUserAsync(Entities.User user);
        System.Threading.Tasks.Task UpdateUserAsync(Entities.User user);
        System.Threading.Tasks.Task DeleteUserAsync(Guid userId);
        System.Threading.Tasks.Task<bool> UserExistsAsync(Guid userId);
        System.Threading.Tasks.Task<bool> UserEmailExistsAsync(string email);
    }
}
