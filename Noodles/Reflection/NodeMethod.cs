
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Noodles.Attributes;
using Noodles.Reflection;
using Walkies;

namespace Noodles
{
    [DebuggerDisplay("{ToString()} - Name={Name}")]
    public class NodeMethod : IGetChild, IInvokeable
    {
        private readonly MethodInfo _methodInfo;

        public Type SignatureType
        {
            get { return Siggs.SiggsExtensions.GetTypeForMethodInfo(_methodInfo); }
        }

        public NodeMethod(object target, MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
            Target = target;
            this.SetParent(target, this.Name);
        }

        public object Target { get; private set; }
        
        private BindingFlags looseBindingFlags = BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public string Name { get { return _methodInfo.Name; } }

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
            var att = this._methodInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
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
                            .GetCustomAttributes(typeof(MessageAttribute), true)
                            .OfType<MessageAttribute>()
                            .FirstOrDefault();
                        if (messageAttribute != null)
                        {
                            _message = messageAttribute.Message;
                        }
                    }
                    _checkedForMessage = true;
                }
                return _message;
            }
        }

        public string Url
        {
            get { return this.Url(); }
        }


        private IEnumerable<NodeMethodParameter> _parameters;

        public bool Active { get { return true; }}

        public IEnumerable<NodeMethodParameter> Parameters
        {
            get
            {
                Func<IEnumerable<NodeMethodParameter>> loadParameters = () =>
                {
                    var parameters =
                        _methodInfo.GetParameters().Select(p => new NodeMethodParameter(this, _methodInfo, p)).ToArray();
                    var methodName = this._methodInfo.Name.StartsWith("set_")
                                         ? _methodInfo.Name.Substring(4)
                                         : _methodInfo.Name;
                    var valuesMethod = Target.GetType().GetMethod(methodName + "_values", looseBindingFlags);
                    if (valuesMethod != null)
                    {
                        var parameterValues =
                            ((IEnumerable<object>) valuesMethod.Invoke(Target, new object[] {})).ToArray();
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

        public object Invoke(object[] parameters)
        {
            var methodParameterInfos = ((IInvokeable)this).Parameters.ToArray();
            for (int index = 0; index < methodParameterInfos.Length; index++)
            {
                var nodeMethodParameter = methodParameterInfos[index];
                var resolvedParameterValue = GetParameterValue(parameters, nodeMethodParameter, index);
                methodParameterInfos[index].LastValue = resolvedParameterValue;
            }
            parameters = methodParameterInfos.Select(mp => mp.LastValue).ToArray();

            return _methodInfo.Invoke(Target, parameters.ToArray());
        }

        private bool? _autoSubmit;

        public bool AutoSubmit
        {
            get
            {
                if (_autoSubmit == null)
                {
                    foreach (var rule in NodeMethodsRuleRegistry.AutoSubmitRules)
                    {
                        _autoSubmit = rule(this._methodInfo);
                        if (_autoSubmit.HasValue) break;
                    }
                    if (_autoSubmit == null)
                    {
                        _autoSubmit = NodeMethodsRuleRegistry.AutoSubmitByDefault;
                    }
                }
                return _autoSubmit.Value;
            }
        }

        public Type ReturnType
        {
            get { return _methodInfo.ReturnType; }
        }


        private object GetParameterValue(object[] parameters, NodeMethodParameter parameterInfo, int index)
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
       

        object IGetChild.this[string name]
        {
            get
            {
                return ((IInvokeable)this).Parameters.SingleOrDefault(p => p.Name == name);
            }
        }


        public bool Equals(NodeMethod other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Path(), this.Path());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(NodeMethod)) return false;
            return Equals((NodeMethod)obj);
        }

        public override int GetHashCode()
        {
            return this.Path().GetHashCode();
        }

        public static bool operator ==(NodeMethod left, NodeMethod right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NodeMethod left, NodeMethod right)
        {
            return !Equals(left, right);
        }

        public T GetAttribute<T>() where T:Attribute
        {
            return (T) _methodInfo.GetCustomAttributes(typeof (T), true).SingleOrDefault();
        }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            var parameters = ((IInvokeable)this).Parameters.Select(p => p.Name).Select(name => parameterDictionary[name]).ToArray();
            return ((IInvokeable)this).Invoke(parameters);
        }
    }

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
        object Invoke(IDictionary<string, object> parameterDictionary);
        object Invoke(object[] parameters);
        T GetAttribute<T>() where T : Attribute;
    }

    public class NotEnoughParametersForNodeMethodException : Exception
    {
        public NodeMethod NodeMethod { get; set; }
        public NodeMethodParameter ParameterInfo { get; set; }
        public object[] Parameters { get; set; }

        public NotEnoughParametersForNodeMethodException(NodeMethod nodeMethod, NodeMethodParameter parameterInfo, object[] parameters)
        {
            NodeMethod = nodeMethod;
            ParameterInfo = parameterInfo;
            Parameters = parameters;
        }
    }
}