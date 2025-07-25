using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Exceptions
{
    public static class GroupException
    {
        public class EmptyNameException : Exception
        {
            public EmptyNameException() : base("Group name cannot be empty.") { }
        }
        public class NameExceedLimitException : Exception
        {
            public NameExceedLimitException(int limit) : base($"Group name cannot exceed {limit} characters.") { }
        }
        public class NameException : Exception
        {
            public NameException(string name) : base($"Group name must be different from the current name: {name}.") { }
        }
        public class EmptyDescriptionException : Exception
        {
            public EmptyDescriptionException() : base("Group description cannot be empty.") { }
        }
        public class DescriptionExceedLimitException : Exception
        {
            public DescriptionExceedLimitException(int limit) : base($"Group description cannot exceed {limit} characters.") { }
        }
        public class DescriptionException : Exception
        {
            public DescriptionException(string description) : base($"Group description must be different from the current description: {description}.") { }
        }

    }
}
