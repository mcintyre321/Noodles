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
    public class ReflectionNodeProperty : IInvokeable, INode, IInvokeableParameter, NodeProperty
    {
        private readonly object _target;
        private readonly PropertyInfo _info;

        public ReflectionNodeProperty(INode parent, object target, PropertyInfo info)
        {
            _target = target;
            _info = info;
            Value = info.GetValue(target, null);
            ValueType = info.PropertyType;
            Name = info.Name;
            DisplayName = GetDisplayName(info);
            var propertyLevelShowAttribute = info.Attributes().OfType<ShowAttribute>().Any();
            var setter = info.GetSetMethod();
            if (setter != null && (propertyLevelShowAttribute || setter.Attributes().OfType<ShowAttribute>().Any()))
            {
                if (Harden.Allow.Set(target, info))
                {
                    Setter = new ReflectionNodeMethod(this, target, setter);
                }
            }
            this.Parent = parent;
        }



        public NodeMethod Setter { get; set; }

        string IInvokeable.DisplayName { get { return "Set " + DisplayName; } }

        object IInvokeableParameter.LastValue
        {
            get { return this.Value; }
            set { this.Value = value; }
        }

        public string DisplayName { get; private set; }

        IEnumerable<Attribute> IInvokeableParameter.CustomAttributes
        {
            get { return this.Attributes().Concat(this._info.Attributes()); }
        }

        public object Value { get; private set; }

        IEnumerable IInvokeableParameter.Choices
        {
            get
            {
                var choicesName = Name + "_choices";
                var choices = _target.GetType().GetMethod(choicesName) ??
                              _target.GetType().GetMethod("get_" + choicesName);
                if (choices != null)
                {
                    return (IEnumerable) choices.Invoke(_target, null);
                }
                return null;
            }
        }

        IEnumerable IInvokeableParameter.Suggestions
        {
            get
            {
                var choicesName = Name + "_suggestions";
                var choices = _target.GetType().GetMethod(choicesName) ??
                              _target.GetType().GetMethod("get_" + choicesName);
                if (choices != null)
                {
                    return (IEnumerable)choices.Invoke(_target, null);
                }
                return null; ;
            }
        }

        bool IInvokeableParameter.Locked
        {
            get { return false; }
        }

        public Type ValueType { get; private set; }

        bool IInvokeableParameter.IsOptional
        {
            get { return true; }
        }

        public Type ParameterType { get { return Siggs.SiggsExtensions.GetTypeForMethodInfo(_info.GetSetMethod()); } }
        public Type ResultType { get { return ValueType; } }
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
        public IEnumerable<IInvokeableParameter> Parameters
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

        string IInvokeableParameter.DisplayName
        {
            get { return DisplayName; }
            set { DisplayName = value; }
        }

        public Uri Url { get { return new Uri(this.Parent.Url + this.Fragment + "/", UriKind.Relative); } }
        public INode Parent { get; set; }
        public string UiHint { get { return CustomAttributes.OfType<ShowAttribute>().Select(a => a.UiHint).SingleOrDefault(); } }
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
            get
            {
                var atts = _info.Attributes();
                var getter = _info.GetGetMethod();
                if (getter != null) atts = atts.Concat(getter.Attributes());
                var setter = _info.GetSetMethod();
                if (setter != null) atts = atts.Concat(setter.Attributes());
                return atts;
            }
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