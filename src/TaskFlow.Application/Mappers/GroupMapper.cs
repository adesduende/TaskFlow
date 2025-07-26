using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Mappers
{
    public static class GroupMapper
    {
        /// <summary>
        /// Maps a group entity to a DTO.
        /// </summary>
        /// <param name="group"> The group entity to map.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"> Thrown when the group is null.</exception>
        public static DTO.GroupDTO ToDto(this Domain.Aggregates.Group group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            return new DTO.GroupDTO(
                group.Id.ToString(),
                group.Name,
                group.Description,
                group.Users.Select(u => u.ToString()).ToList(),
                group.Tasks.Select(t => t.ToString()).ToList(),
                group.Owner.ToString()
            );
        }
    }
}
