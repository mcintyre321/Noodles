using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Noodles.Attributes;
using Noodles.Models;

namespace Noodles
{
    public class NodeMethodParameter : IInvokeableParameter
    {
        private readonly NodeMethod _nodeMethod;
        private readonly MethodInfo _mi;
        private readonly ParameterInfo _parameterInfo;

        private BindingFlags looseBindingFlags = BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private object _value;

        internal NodeMethodParameter(NodeMethod nodeMethod, MethodInfo mi, ParameterInfo parameterInfo, int order)
        {
            _nodeMethod = nodeMethod;
            _mi = mi;
            _parameterInfo = parameterInfo;
            Parent = nodeMethod;
            Order = order;
        }

        public Type ValueType
        {
            get
            {
                return _parameterInfo.ParameterType;
            }
        }

        public bool IsOptional
        {
            get { return _parameterInfo.IsOptional; }
        }

        public Type Type { get { return this.GetType(); } }

        public int Order { get; private set; }
        public object Invoke(object[] objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NodeMethod> NodeMethods { get { yield break; } }
        public IEnumerable<NodeProperty> NodeProperties { get { yield break; } }
        public IEnumerable<Resource> Children { get { yield break; } }

        public string Name
        {
            get
            {
                return _parameterInfo.Name;
            }
        }

        public object Value
        {
            get
            {
                if (_value != null) return _value;
                if (_mi.Name.StartsWith("set_") && !_mi.Name.EndsWith("_callback"))
                {
                    var property = _nodeMethod.TargetObject.GetType().GetProperty(_mi.Name.Substring(4), looseBindingFlags);
                    var getter = property.GetGetMethod(true);
                    return getter.Invoke(_nodeMethod.TargetObject, null);
                }
                var getDefault = _mi.DeclaringType.GetMethod(_mi.Name + "_" + _parameterInfo.Name + "_default");
                if (getDefault != null)
                {
                    return getDefault.Invoke(_nodeMethod.TargetObject, null);
                }
                else
                {
                    var defaultAttribute = _parameterInfo.Attributes().OfType<DefaultAttribute>().SingleOrDefault();
                    if (defaultAttribute != null)
                    {
                        return defaultAttribute.GetValue(_nodeMethod.TargetObject);
                    }
                }
                return null;
            }
            set { _value = value; }
        }

        public IEnumerable Choices
        {
            get
            {
                string methodName = "";
                if (!_nodeMethod.Name.StartsWith("set_"))
                {
                    methodName = _nodeMethod.Name + "_";
                }

                methodName = methodName + Name + "_choices";
                var choices = _nodeMethod.TargetObject.GetType().GetMethod(methodName, looseBindingFlags)
                    ?? _nodeMethod.TargetObject.GetType().GetMethod("get_" + methodName, looseBindingFlags);
                if (choices != null)
                {
                    return (IEnumerable)choices.Invoke(_nodeMethod.TargetObject, null);
                }
                return null;
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get
            {
                return _displayName ?? (_displayName = GetDisplayName());
            }
            set { _displayName = value; }
        }

        public Uri Url
        {
            get { return new Uri(Parent.Url + Fragment + "/", UriKind.Relative); }
        }

        public INode Parent { get; set; }
        public string UiHint { get; private set; }

        string GetDisplayName()
        {
            var att = this._parameterInfo.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true).OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }

        public IEnumerable Suggestions
        {
            get
            {
                string methodName = "";
                if (!_nodeMethod.Name.StartsWith("set_"))
                {
                    methodName = _nodeMethod.Name + "_";
                }
                methodName = methodName + Name + "_suggestions";
                var suggestions = _nodeMethod.TargetObject.GetType().GetMethod(methodName, looseBindingFlags)
                    ?? _nodeMethod.TargetObject.GetType().GetMethod("get_" + methodName, looseBindingFlags);
                if (suggestions != null)
                {
                    return (IEnumerable)suggestions.Invoke(_nodeMethod.TargetObject, null);
                }
                return null;
            }
        }

        public IEnumerable<string> ErrorMessages { get; private set; }

        internal ParameterInfo ParameterInfoInfo
        {
            get { return this._parameterInfo; }
        }

        public NodeMethod NodeMethod { get { return _nodeMethod; } }

        public IEnumerable<Attribute> Attributes
        {
            get
            {
                var parameterAtts = this._parameterInfo.GetCustomAttributes(false).Cast<Attribute>();
                if (_mi.Name.StartsWith("set_"))
                {
                    var propertyAtts = _mi.DeclaringType.GetProperty(_mi.Name.Substring(4))
                        .GetCustomAttributes().Cast<Attribute>();
                    var methodAtts = _mi.GetCustomAttributes(false).Cast<Attribute>();
                    return parameterAtts.Concat(methodAtts).Concat(propertyAtts);
                }
                return parameterAtts;
            }
        }

        public bool Locked { get; set; }
        public bool Readonly { get { return false; } }
        public IEnumerable<string> GetValidationErrorsForValue(object value)
        {
            yield break;
        }

        public IEnumerable<object> ChildNodes { get{ yield break;} }

        public Resource GetChild(string name)
        {
            return null;
        }

         

        public string Fragment { get { return Name; }}
    }
}