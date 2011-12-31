using System;
using System.Collections;
using System.Reflection;
using FormFactory;

namespace WebNoodle.Reflection
{
    public class ObjectMethodParameter
    {
        private readonly ObjectMethod _objectMethod;
        private readonly object _target;
        private readonly MethodInfo _mi;
        private readonly ParameterInfo _parameter;

        private BindingFlags looseBindingFlags = BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy |
                                                 BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private object _value;

        internal ObjectMethodParameter(ObjectMethod objectMethod, object target, MethodInfo mi, ParameterInfo parameter)
        {
            _objectMethod = objectMethod;
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
                if (typeof(INode).IsAssignableFrom(_parameter.ParameterType)) return typeof(String);
                return _parameter.ParameterType;
            }
        }

        public string Name { get { return _parameter.Name; } }

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
                var methodName = _objectMethod.Name.StartsWith("set_") ? _objectMethod.Name.Substring(4) : _objectMethod.Name;
                
                var choices = _target.GetType().GetMethod(methodName + "_" + Name + "_choices",
                                                                looseBindingFlags);
                if (choices != null)
                {
                    return  (IEnumerable)choices.Invoke(_target, null);
                }
                return null;
            }
        }

        public string DisplayName
        {
            get { return Name.Sentencise(); }
        }

        public IEnumerable Suggestions
        {
            get
            {
                var methodName = _objectMethod.Name.StartsWith("set_") ? _objectMethod.Name.Substring(4) : _objectMethod.Name;
                var suggestions = _target.GetType().GetMethod(methodName + "_" + Name + "_suggestions",
                                                              looseBindingFlags);
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
    }
}