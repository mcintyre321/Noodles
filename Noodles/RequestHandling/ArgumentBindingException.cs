using System;
using System.Collections.Generic;

namespace Noodles.RequestHandling
{
    public abstract class ArgumentBindingException : Exception
    {
        public ArgumentBindingException()
        {
        }

        public abstract IEnumerable<KeyValuePair<string, string>> Errors { get; }
    }
}