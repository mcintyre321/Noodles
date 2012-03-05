using System;

namespace Noodles
{
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
        }
    }
}