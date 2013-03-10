using System.Collections.Generic;
using System.Threading.Tasks;
using Noodles.Models;

namespace Noodles.RequestHandling
{
    public abstract class RequestInfo
    {
        public bool IsInvoke { get; protected set; }
        public abstract string RootUrl { get;  }

        public abstract Task<IEnumerable<object>> GetArguments(IInvokeable method);
    }
}