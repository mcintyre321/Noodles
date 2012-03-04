using System.Collections.Generic;

namespace WebNoodle
{
    public interface IObjectMethod : IHasPath
    {
        string Name { get; }
        string DisplayName { get; }
        IEnumerable<ObjectMethodParameter> Parameters { get; }
        object Target { get; }
        string SuccessMessage { get; }
        void Invoke(object[] parameters);
    }
}