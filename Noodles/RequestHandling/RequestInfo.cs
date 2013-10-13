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

        public abstract Task<ArgumentBinding[]> GetArgumentBindings(IInvokeable method);
    }

    public class ArgumentBinding
    {
        public IInvokeableParameter Parameter { get; set; }
        public object Value { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}