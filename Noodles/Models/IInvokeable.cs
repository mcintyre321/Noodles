using System;
using System.Collections;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface IInvokeable 
    {
        bool Active { get; }
        IEnumerable<IInvokeableParameter> Parameters { get; }
        string InvokeDisplayName { get; }
        Uri InvokeUrl { get; }
        object Target { get; }
        string Message { get; }
        bool AutoSubmit { get; }
        Type ParameterType { get; }
        Type ResultType { get; }
        //object Parameter { get; }
        object Invoke(IDictionary<string, object> parameterDictionary);
        object Invoke(object[] parameters);
        T GetAttribute<T>() where T : Attribute;
    }

    
}