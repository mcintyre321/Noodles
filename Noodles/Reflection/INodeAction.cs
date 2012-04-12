using System.Collections.Generic;

namespace Noodles
{
    public interface INodeAction : IHasPath
    {
        string Name { get; }
        string DisplayName { get; }
        IEnumerable<NodeActionParameter> Parameters { get; }
        object Target { get; }
        string SuccessMessage { get; }
        void Invoke(object[] parameters);
    }
}