using System.Collections.Generic;

namespace Noodles
{
    public interface INodeMethod
    {
        string Name { get; }
        string DisplayName { get; set; }
        IEnumerable<NodeMethodParameter> Parameters { get; }
        object Target { get; }
        string SuccessMessage { get; }
        void Invoke(object[] parameters);
    }
}