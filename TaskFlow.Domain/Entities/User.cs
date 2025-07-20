using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; set; }

        public User(Guid id, string name, string email)
            : base(id)
        {
            Name = name;
            Email = email;
        }
        /// <summary>
        /// Update the user's name
        /// </summary>
        /// <param name="name"> The name of the user</param>
        /// <exception cref="ArgumentException"> Empty name</exception>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Name cannot exceed 100 characters.", nameof(name));
            if (name == Name)
                throw new ArgumentException("Name must be different from the current name.", nameof(name));
            Name = name;
        }
    }
}
