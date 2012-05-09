using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noodles.Attributes
{
    public class MessageAttribute : Attribute
    {
        private readonly string _message;
        public string Message { get { return _message; } }
        
        public MessageAttribute(string message)
        {
            this._message = message;
        }
    }
}
