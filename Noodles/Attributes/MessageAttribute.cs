using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noodles.Attributes
{
   public  class MessageAttribute : Attribute
    {
       public string Message { get; private set; }

       public MessageAttribute(string message)
       {
           Message = message;
       }
    }
}
