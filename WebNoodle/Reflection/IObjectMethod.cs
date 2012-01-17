using System.Collections.Generic;

namespace WebNoodle.Reflection
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