using System;
using System.Collections.Generic;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class FormInfo : IInvokeable
    {
        private readonly IInvokeable _resource;
        private readonly IEnumerable<IInvokeableParameter> _formParameters;

        public INode GetChild(string name)
        {
            return _resource.GetChild(name);
        }

        public INode Parent
        {
            get { return _resource.Parent; }
        }

        public bool Active
        {
            get { return _resource.Active; }
        }

        public IEnumerable<IInvokeableParameter> Parameters
        {
            get { return _formParameters; }
        }

        public string DisplayName
        {
            get { return _resource.DisplayName; }
        }

        public object Target
        {
            get { return _resource.Target; }
        }

        public string Message
        {
            get { return _resource.Message; }
        }

        public string Name
        {
            get { return _resource.Name; }
        }

        public Uri Url
        {
            get { return _resource.Url; }
        }

        public bool AutoSubmit
        {
            get { return _resource.AutoSubmit; }
        }

        public Type ParameterType
        {
            get { return _resource.ParameterType; }
        }

        public Type ResultType { get { return _resource.ResultType; } }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            return _resource.Invoke(parameterDictionary);
        }

        public object Invoke(object[] parameters)
        {
            return _resource.Invoke(parameters);
        }

        public T GetAttribute<T>() where T : Attribute
        {
            return _resource.GetAttribute<T>();
        }

        public FormInfo(IInvokeable resource, IEnumerable<IInvokeableParameter> formParameters)
        {
            this._resource = resource;
            _formParameters = formParameters;
        }
    }
}