using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DelegateQueryable;
using Noodles.Helpers;

namespace Noodles.Models
{
    [DisplayName("{DisplayName}")]
    public class NodeProperty : IInvokeable, INode
    {
        private readonly object _target;
        private readonly PropertyInfo _info;

        public NodeProperty(INode parent, object target, PropertyInfo info)
        {
            _target = target;
            _info = info;
            Value = info.GetValue(target, null);
            ValueType = info.PropertyType;
            Name = info.Name;
            DisplayName = GetDisplayName(info);
            var setter = info.GetSetMethod();
            if (setter != null)
            {
                if (Harden.Allow.Set(target, info))
                {
                    Setter = new NodeMethod(this, target, setter);
                }
            }
            this.Parent = parent;
        }



        public NodeMethod Setter { get; set; }

        string IInvokeable.DisplayName { get { return "Set " + DisplayName; } }
        public string DisplayName { get; private set; }
        public object Value { get; private set; }
        public Type ValueType { get; private set; }
        public Type ParameterType { get { return ValueType; } }
        public object Parameter { get; private set; }
        public Type Type { get { return this.GetType(); } }

        public int Order
        {
            get
            {
                return this._info.Attributes().OfType<ShowAttribute>().Select(a => a.UiOrder as int?).SingleOrDefault() ?? int.MaxValue;
            }
        }

        public bool Active { get { return !Readonly; } }
        public IEnumerable<NodeMethodParameter> Parameters
        {
            get
            {
                if (Setter == null) return Enumerable.Empty<NodeMethodParameter>();
                return Setter.Parameters.Then(p => p.DisplayName = this.DisplayName);
            }
        }
        public string Name { get; private set; }

        public object Target
        {
            get { return _target; }
        }

        public string Url { get { return this.Parent.Url + this.Fragment + "/"; } }
        public INode Parent { get; set; }
        public string UiHint { get { return _info.Attributes().OfType<ShowAttribute>().Select(a => a.UiHint).SingleOrDefault(); } }
        public string TypeName { get { return "NodeProperty"; } }
        public bool AutoSubmit { get { return false; } }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            return Setter.Invoke(parameterDictionary);
        }
        public string Message { get { return ""; } }

        public object Invoke(object[] parameters)
        {
            return Setter.Invoke(parameters);
        }

        T IInvokeable.GetAttribute<T>()
        {
            return ((Setter == null) ? null as T : Setter.GetAttribute<T>()) ?? this.CustomAttributes.OfType<T>().SingleOrDefault();
        }

        public IEnumerable<object> CustomAttributes
        {
            get { return _info.Attributes(); }
        }

        public bool Readonly { get { return Setter == null; } }

        string GetDisplayName(PropertyInfo info)
        {
            var att = info.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true).OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }


        public virtual INode GetChild(string fragment)
        {
            return NodeProperties.SingleOrDefault(p => p.Fragment == fragment) as INode
            ?? NodeMethods.SingleOrDefault(p => p.Fragment == fragment);
        }


        public virtual IEnumerable<NodeMethod> NodeMethods
        {
            get { yield break; }
        }

        public IEnumerable<NodeProperty> NodeProperties { get { yield break; } }
        public IEnumerable<Resource> Children { get { yield break; } }

        public string Fragment { get { return Name; } }
    }
}