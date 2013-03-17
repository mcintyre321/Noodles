using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class ClickToEditAttribute : Attribute
    {
    }

    public static class NodeMethodGetFormGroupsExtension
    {
        public static IEnumerable<FormInfo> GetFormGroups(this IInvokeable invokeable)
        {
            var singleFormParameters =
                invokeable.Parameters.Where(x => !x.CustomAttributes.OfType<ClickToEditAttribute>().Any());
            if (singleFormParameters.Any())
                yield return new FormInfo(invokeable, singleFormParameters);
        }
        public static IEnumerable<IInvokeableParameter> GetSingleSettableProperies(this IInvokeable invokeable)
        {
            return invokeable.Parameters.Where(x => x.CustomAttributes.OfType<ClickToEditAttribute>().Any());

        }
    }


    public class FormInfo : IInvokeable
    {
        private readonly IInvokeable _resource;
        private readonly IEnumerable<IInvokeableParameter> _formParameters;

        public INode GetChild(string fragment)
        {
            return _resource.GetChild(fragment);
        }

        public string Fragment
        {
            get { return _resource.Fragment; }
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

        public string Url
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