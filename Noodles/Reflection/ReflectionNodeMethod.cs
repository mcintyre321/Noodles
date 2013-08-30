using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Noodles;
using Noodles.Reflection;

namespace Noodles.Models
{
    [DebuggerDisplay("{ToString()} - Name={Name}")]
    public class ReflectionNodeMethod : NodeMethod
    {
        private readonly MethodInfo _methodInfo;


        public Type Type { get { return this.GetType(); } }
        public int Order { get; private set; }

        public ReflectionNodeMethod(INode parent, object target, MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
            Order = int.MaxValue;
            Parent = parent;
            Target = target;
            UiHint = methodInfo.Attributes().OfType<ShowAttribute>().Select(a => a.UiHint).SingleOrDefault() ?? "";
        }

        public Uri InvokeUrl { get { return Url; } }
        public object Target { get; private set; }

        private BindingFlags looseBindingFlags = BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public string Name { get { return _methodInfo.Name; } }

        string IInvokeable.InvokeDisplayName { get { return DisplayName; } }
        public string DisplayName
        {
            get
            {
                return _displayName ?? (_displayName = GetDisplayName());
            }
            set { _displayName = value; }
        }


        private string GetDisplayName()
        {
            var att = this._methodInfo.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true).OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }

        private string _displayName;


        private bool _checkedForMessage = false;
        private string _message;
        public string Message
        {
            get
            {
                if (!_checkedForMessage)
                {
                    // TODO: if a set property?
                    string messageMethodName = Name + "_message";
                    var messageMethod = this.Target.GetType().GetMethod(messageMethodName, looseBindingFlags)
                                        ?? this.Target.GetType().GetMethod("get_" + messageMethodName, looseBindingFlags);
                    if (messageMethod != null)
                    {
                        _message = (string)messageMethod.Invoke(this.Target, null);
                    }
                    else
                    {
                        var messageAttribute = this._methodInfo
                                                   .GetCustomAttributes(typeof(DescriptionAttribute), true)
                                                   .OfType<DescriptionAttribute>()
                                                   .FirstOrDefault();
                        if (messageAttribute != null)
                        {
                            _message = messageAttribute.Description;
                        }
                    }
                    _checkedForMessage = true;
                }
                return _message;
            }
        }

        public Uri Url
        {
            get { return new Uri(this.Parent.Url +  this.Fragment + "/", UriKind.Relative); }
        }

        public INode Parent { get; private set; }
        public Type ValueType { get { return typeof(NodeMethod); } }
        public IEnumerable<Attribute> Attributes { get { return _methodInfo.Attributes(); } }
        public string UiHint { get; private set; }

        private IEnumerable<IInvokeableParameter> _parameters;

        public bool Active { get { return true; } }

        public IEnumerable<IInvokeableParameter> Parameters
        {
            get
            {
                Func<IEnumerable<IInvokeableParameter>> loadParameters = () =>
                {
                    var parameters =
                        _methodInfo.GetParameters().Select((p, i) => new NodeMethodParameter(this, _methodInfo, p, i)).ToArray();
                    var methodName = this._methodInfo.Name.StartsWith("set_")
                                         ? _methodInfo.Name.Substring(4)
                                         : _methodInfo.Name;
                    var valuesMethod = Target.GetType().GetMethod(methodName + "_values", looseBindingFlags);
                    if (valuesMethod != null)
                    {
                        var parameterValues =
                            ((IEnumerable<object>)valuesMethod.Invoke(Target, new object[] { })).ToArray();
                        for (int i = 0; i < parameterValues.Length; i++)
                        {
                            parameters[i].Value = parameterValues[i];
                        }
                    }
                    return parameters;
                };

                return _parameters ?? (_parameters = loadParameters());
            }
        }
         

       


        public IEnumerable<INode> ChildNodes { get { return this.Parameters; } }

        public Type ReturnType
        {
            get { return _methodInfo.ReturnType; }
        }
        public IEnumerable<NodeMethod> NodeMethods { get
        {
            return this.GetNodeMethods(this);
        } }

        public IEnumerable<NodeProperty> NodeProperties
        {
            get { yield break; }
        }

        public IEnumerable<Resource> Children { get { yield break; } }


        private object GetParameterValue(object[] parameters, IInvokeableParameter parameterInfo, int index)
        {
            foreach (var fix in NodeMethodParameterFixes.Registry)
            {
                object result;
                if (fix(parameters, parameterInfo, index, out result))
                {
                    return result;
                }
            }
            if (parameters.Length < index)
            {
                throw new NotEnoughParametersForNodeMethodException(this, parameterInfo, parameters);
            }

            return parameters[index];
        }

        public Resource GetChild(string fragment)
        {
            return null;
        }

        public string Fragment { get { return Name; } }

        public Type ParameterType
        {
            get { return Siggs.SiggsExtensions.GetTypeForMethodInfo(_methodInfo); }
        }

        public Type ResultType { get { return _methodInfo.ReturnType; } }

        public object Parameter { get { return Activator.CreateInstance(ParameterType); } }


        public T GetAttribute<T>() where T : Attribute
        {
            return (T)_methodInfo.GetCustomAttributes(typeof(T), true).SingleOrDefault();
        }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            var parameters = ((IInvokeable)this).Parameters.Select(p => p.Name).Select(name => parameterDictionary[name]).ToArray();
            return Invoke(parameters);
        }

        private object Invoke(object[] parameters)
        {
            var methodParameterInfos = ((IInvokeable) this).Parameters.ToArray();
            var parameterValues = new object[methodParameterInfos.Length];
            for (int index = 0; index < methodParameterInfos.Length; index++)
            {
                var nodeMethodParameter = methodParameterInfos[index];
                var resolvedParameterValue = GetParameterValue(parameters, nodeMethodParameter, index);
                parameterValues[index] = resolvedParameterValue;
            }

            return _methodInfo.Invoke(Target, parameterValues);
        }
    }
}