using System;
using System.Collections;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface IInvokeable 
    {
        IEnumerable<IInvokeableParameter> Parameters { get; }
        string InvokeDisplayName { get; }
        Uri InvokeUrl { get; }
        object Target { get; }
        Type ParameterType { get; }
        Type ResultType { get; }
        //object Parameter { get; }
        object Invoke(IDictionary<string, object> parameterDictionary);
        T GetAttribute<T>() where T : Attribute;
    }

    
}