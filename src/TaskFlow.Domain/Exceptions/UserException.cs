using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Exceptions
{
    public static class UserException
    {
        [Serializable]
        public class EmptyNameException : Exception
        {
            public EmptyNameException() : base("Name cannot be empty.") { }
        }
        [Serializable]
        public class NameExceedLimitException : Exception
        {
            public NameExceedLimitException(int limit) : base($"Name cannot exceed {limit} characters.") { }
        }
        [Serializable]
        public class NameException : Exception
        {
            public NameException(string name) : base($"Name must be different from the current name: {name}.") { }
        }
        [Serializable]
        public class EmptyEmailException : Exception
        {
            public EmptyEmailException() : base("Email cannot be empty.") { }
        }
        [Serializable]
        public class InvalidEmailFormatException : Exception
        {
            public InvalidEmailFormatException(string email) : base($"Invalid email format: {email}.") { }
        }

        [Serializable]
        internal class EmptyPasswordException : Exception
        {
            public EmptyPasswordException() : base("Password cannot be empty.") { }
        }
    }
}
