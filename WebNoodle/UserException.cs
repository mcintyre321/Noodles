using System;

namespace WebNoodle
{
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
        }
    }
}