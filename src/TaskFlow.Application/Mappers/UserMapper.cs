using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Mappers
{
    public static class UserMapper
    {
        /// <summary>
        /// Maps a user entity to a DTO.
        /// </summary>
        /// <param name="user">The user entity to map.</param>
        /// <returns>A DTO representing the user.</returns>
        public static DTO.UserDTO ToDto(this Domain.Entities.User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return new DTO.UserDTO(
                user.Id.ToString(),
                user.Name,
                user.Email,
                user.Password
            );
        }
    }
}
