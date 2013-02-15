using System.Collections.Generic;
using System.Threading.Tasks;

namespace Noodles.Requests
{
    public abstract class NoodlesRequest
    {
        public bool IsInvoke { get; private set; }
        public abstract string RootUrl { get;  }

        public abstract Task<IEnumerable<object>> GetArguments(IInvokeable method);
    }
}