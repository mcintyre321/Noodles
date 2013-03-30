using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noodles.Models;

namespace Noodles.RequestHandling
{
    public abstract class RequestInfo
    {
        public abstract bool IsInvoke(IInvokeable invokeable);
        public abstract Uri RootUrl { get;  }

        public abstract Task<IEnumerable<Tuple<string, object>>> GetArguments(IInvokeable method);
    }
}