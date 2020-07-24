using System;

namespace WebAPI_DotNetCore_Demo.Application.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException() { }

        public IncorrectPasswordException(string message) : base(message) { }

        public IncorrectPasswordException(string message, Exception inner) : base(message, inner) { }
    }
}
