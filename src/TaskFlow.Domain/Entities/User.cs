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
        public string Email { get; private set; }
        public string Password { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with a unique identifier, name, email, and password.
        /// </summary>
        /// <param name="id"> The unique identifier for the user.</param>
        /// <param name="name"> The name of the user.</param>
        /// <param name="email"> The email of the user.</param>
        /// <param name="password"> The password of the user.</param>
        public User(Guid id, string name, string email, string password)
            : base(id)
        {
            // Validate and initialize properties
            Email = String.Empty;
            Name = String.Empty;
            Password = String.Empty;

            // Set the properties using the provided methods
            UpdateName(name);
            SetEmail(email);
            UpdatePassword(password);
        }
        /// <summary>
        /// Set the user's email
        /// </summary>
        /// <param name="email"> The email of the user</param>
        /// <exception cref="UserException.EmptyEmailException"> Thrown when the email is empty or null.</exception>
        /// <exception cref="UserException.InvalidEmailFormatException"> Thrown when the email format is invalid.</exception>
        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new UserException.EmptyEmailException();
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new UserException.InvalidEmailFormatException(email);
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
        /// <summary>
        /// Update the user's password
        /// </summary>
        /// <param name="password"> The password of the user</param>
        /// <exception cref="UserException.EmptyPasswordException"> Thrown when the password is empty or null.</exception>
        public void UpdatePassword(string password)
        { 
            if(string.IsNullOrWhiteSpace(password))
                throw new UserException.EmptyPasswordException();

            Password = password;
        }


    }
}
