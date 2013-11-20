using System;

namespace Noodles
{
    public class UserException : Exception
    {
        public UserException(string message)
            : this(message, "")
        {
        }
        public UserException(string message, string memberName)
            : base(message)
        {
            MemberName = memberName;
        }

        public string MemberName { get; private set; }
    }
}