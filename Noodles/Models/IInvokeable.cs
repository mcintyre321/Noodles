using System;
using System.Collections;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface IInvokeable : INode
    {
        bool Active { get; }
        IEnumerable<IInvokeableParameter> Parameters { get; }
        string Name { get; }
        string DisplayName { get; }
        object Target { get; }
        string Message { get; }
        Uri Url { get; }
        bool AutoSubmit { get; }
        Type ParameterType { get; }
        Type ResultType { get; }
        //object Parameter { get; }
        object Invoke(IDictionary<string, object> parameterDictionary);
        object Invoke(object[] parameters);
        T GetAttribute<T>() where T : Attribute;
    }
}