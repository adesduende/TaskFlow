using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get
            {
                return Email;
            }
            private set { 
                if (string.IsNullOrWhiteSpace(value))
                    throw new UserException.EmptyEmailException();
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new UserException.InvalidEmailFormatException(value);
                Email = value;
            } 
        }

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
                throw new UserException.EmptyNameException();
            if (name.Length > 100)
                throw new UserException.NameExceedLimitException(100);
            if (name == Name)
                throw new UserException.NameException(Name);
            Name = name;
        }
    }
}
