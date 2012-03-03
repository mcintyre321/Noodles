using System.Collections.Generic;

namespace WebNoodle
{
    public interface IObjectMethod
    {
        string Name { get; }
        string DisplayName { get; }
        IEnumerable<ObjectMethodParameter> Parameters { get; }
        object Target { get; }
        void Invoke(object[] parameters);
    }
}