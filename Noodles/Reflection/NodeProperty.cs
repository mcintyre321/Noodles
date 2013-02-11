using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Noodles.Helpers;
using Walkies;

namespace Noodles
{
    public class NodeProperty : IInvokeable
    {
        private readonly object _target;
        private readonly Func<IEnumerable<Attribute>> _getCustomAttributes; 
        public NodeProperty(object target, PropertyInfo info)
        {
            _target = target;
            _getCustomAttributes = info.GetCustomAttributes;
            Value = info.GetValue(target, null);
            PropertyType = info.PropertyType;
            Name = info.Name;
            DisplayName = GetDisplayName(info);
            var setter = info.GetSetMethod();
            if (setter != null)
            {
                if (Harden.Allow.Set(target, info))
                {
                    Setter = new NodeMethod(target, setter);
                }
            }
            this.SetParent(_target, this.Name);

        }

        public NodeProperty(object target, FieldInfo info)
        {
            _target = target;
            _getCustomAttributes = info.GetCustomAttributes;
            Value = info.GetValue(target);
            PropertyType = info.FieldType;
            Name = info.Name;
            DisplayName = GetDisplayName(info);
            this.SetParent(_target, this.Name);
        }

        public NodeMethod Setter { get; set; }

        string IInvokeable.DisplayName { get { return "Set " + DisplayName; } }
        public string DisplayName { get; private set; }
        public Type PropertyType { get; private set; }
        public object Value { get; private set; }

        public bool Active { get { return !Readonly; } }
        public IEnumerable<NodeMethodParameter> Parameters { get{ return Setter.Parameters.Then(p => p.DisplayName = this.DisplayName);}}
        public string Name { get; private set; }

        public object Target
        {
            get { return _target; }
        }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            return Setter.Invoke(parameterDictionary);
        }
        public string Message { get { return "";  } }

        public object Invoke(object[] parameters)
        {
            return Setter.Invoke(parameters);
        }

        T IInvokeable.GetAttribute<T>() 
        {
            return Setter.GetAttribute<T>() ?? this.CustomAttributes.OfType<T>().SingleOrDefault();
        }

        public IEnumerable<object> CustomAttributes
        {
            get { return _getCustomAttributes(); }
        }

        public bool Readonly { get { return Setter == null; } }

        string GetDisplayName(PropertyInfo info)
        {
            var att = info.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }
        string GetDisplayName(FieldInfo info)
        {
            var att = info.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }
    }
}