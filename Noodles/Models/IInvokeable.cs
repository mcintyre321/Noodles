using System;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface IInvokeable
    {
        bool Active { get; }
        IEnumerable<NodeMethodParameter> Parameters { get; }
        string Name { get; }
        string DisplayName { get; }
        object Target { get; }
        string Message { get; }
        string Url { get; }
        bool AutoSubmit { get; }
        Type ValueType { get; }
        object Invoke(IDictionary<string, object> parameterDictionary);
        object Invoke(object[] parameters);
        T GetAttribute<T>() where T : Attribute;
    }
}