using System;

namespace Noodles
{
    public class NodeNotFoundException : Exception
    {
        public NodeNotFoundException(string message) : base(message)
        {
        }
    }
}