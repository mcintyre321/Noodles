using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class NodeActionParameter
    {
        private readonly NodeAction _nodeAction;
        private readonly object _target;
        private readonly MethodInfo _mi;
        private readonly ParameterInfo _parameter;

        private BindingFlags looseBindingFlags = BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy |
                                                 BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private object _value;

        internal NodeActionParameter(NodeAction nodeAction, object target, MethodInfo mi, ParameterInfo parameter)
        {
            _nodeAction = nodeAction;
            _target = target;
            _mi = mi;
            _parameter = parameter;
        }

        public Type ParameterType
        {
            get
            {
                return _parameter.ParameterType;
            }
        }
        public Type BindingParameterType
        {
            get
            {
                if (typeof(IHasName).IsAssignableFrom(_parameter.ParameterType)) return typeof(String);
                return _parameter.ParameterType;
            }
        }

        public string Name
        {
            get
            {
                if (_mi.Name.StartsWith("set_") && _parameter.Name == "value")
                    return _mi.Name.Substring(4); 
                return _parameter.Name;
            }
        }

        public object Value
        {
            get
            {
                if (_value != null) return _value;
                if (_mi.Name.StartsWith("set_") && !_mi.Name.EndsWith("_callback"))
                {
                    var property = _target.GetType().GetProperty(_mi.Name.Substring(4), looseBindingFlags);
                    var getter = property.GetGetMethod(true);
                    return getter.Invoke(_target, null);
                }
                return null;
            }
            set { _value = value; }
        }

        public object LastValue { get; set; }
        public IEnumerable Choices
        {
            get
            {
                string methodName = "";
                if (!_nodeAction.Name.StartsWith("set_"))
                {
                    methodName = _nodeAction.Name + "_";
                }

                methodName = methodName + Name + "_choices";
                var choices = _target.GetType().GetMethod(methodName, looseBindingFlags)
                    ?? _target.GetType().GetMethod("get_" + methodName, looseBindingFlags);
                if (choices != null)
                {
                    return  (IEnumerable)choices.Invoke(_target, null);
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
        }
        string GetDisplayName()
        {
            var att = this._parameter.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
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
                if (!_nodeAction.Name.StartsWith("set_"))
                {
                    methodName = _nodeAction.Name + "_";
                }
                methodName = methodName + Name + "_suggestions";
                var suggestions = _target.GetType().GetMethod(methodName, looseBindingFlags)
                    ?? _target.GetType().GetMethod("get_" + methodName, looseBindingFlags);
                if (suggestions != null)
                {
                    return (IEnumerable)suggestions.Invoke(_target, null);
                }
                return null;
            }
        }

        internal ParameterInfo ParameterInfo
        {
            get { return this._parameter; }
        }

        public INodeAction NodeAction { get { return _nodeAction; } }

        public IEnumerable<Attribute> CustomAttributes
        {
            get { return this._parameter.GetCustomAttributes(false).Cast<Attribute>(); }
        }
    }
}