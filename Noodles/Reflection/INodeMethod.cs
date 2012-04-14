using System.Collections.Generic;

namespace Noodles
{
    public interface INodeMethod : IHasPath
    {
        string Name { get; }
        string DisplayName { get; }
        IEnumerable<NodeMethodParameter> Parameters { get; }
        object Target { get; }
        string SuccessMessage { get; }
        void Invoke(object[] parameters);
    }
}