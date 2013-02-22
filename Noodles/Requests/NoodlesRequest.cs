using System.Collections.Generic;
using System.Threading.Tasks;
using Noodles.Models;

namespace Noodles.Requests
{
    public abstract class NoodlesRequest
    {
        public bool IsInvoke { get; protected set; }
        public abstract string RootUrl { get;  }

        public abstract Task<IEnumerable<object>> GetArguments(IInvokeable method);
    }
}