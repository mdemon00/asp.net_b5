using System;

namespace LibraryManagementSystem.Reading.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message)
            : base(message) { }
    }
}
