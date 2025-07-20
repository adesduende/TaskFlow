using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Exceptions
{
    public static class TaskException
    {
        public class EmptyTitleException : Exception
        {
            public EmptyTitleException() : base("Title cannot be empty.") { }
        }

        public class TitleExceedLimitException : Exception
        {
            public TitleExceedLimitException(int rateLimit) : base($"Title cannot exceed {rateLimit} characters.") { }
        }

        public class TitleException : Exception
        {
            public TitleException(string title) : base($"Title must be different from the current title: {title}.") { }
        }

        public class EmptyDescriptionException : Exception
        {
            public EmptyDescriptionException() : base("Description cannot be empty.") { }
        }
        public class DescriptionExceedLimitException : Exception
        {
            public DescriptionExceedLimitException(int rateLimit) : base($"Description cannot exceed {rateLimit} characters.") { }
        }
        public class DescriptionException : Exception
        {
            public DescriptionException(string description) : base($"Description must be different from the current description: {description}.") { }
        }
        public class EmptyPriorityException : Exception
        {
            public EmptyPriorityException() : base("Priority cannot be empty.") { }
        }
        public class EmptyStatusException : Exception
        {
            public EmptyStatusException() : base("Status cannot be empty.") { }
        }
    }
}
