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
    public class ReflectionNodeProperty : IInvokeable, NodeProperty 
    {
        private readonly object _target;
        private readonly PropertyInfo _info;
        
        public ReflectionNodeProperty(INode parent, object target, PropertyInfo info)
        {
            _target = target;
            _info = info;
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

        string IInvokeable.InvokeDisplayName { get { return "Set " + DisplayName; } }
        public Uri InvokeUrl { get { return Url; } }
        INode INode.Parent { get { return Parent; } }

        public INode Parent { get; private set; }

   
 

        public string DisplayName { get; private set; }

       
        public object Value
        {
            get { return _info.GetValue(_target); }
            private set { _info.SetValue(_target, value); }
        }

        public IEnumerable Choices
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

        public Type ValueType { get; private set; }


         
        public Type ParameterType { get { return Siggs.SiggsExtensions.GetTypeForMethodInfo(_info.GetSetMethod()); } }
        public Type ResultType { get { return ValueType; } }
        public object Parameter { get; private set; }
        public Type Type { get { return this.GetType(); } }

        

        public bool Active { get { return !Readonly; } }
        public IEnumerable<IInvokeableParameter> Parameters
        {
            get
            {
                if (Setter == null) return Enumerable.Empty<NodeMethodParameter>();
                return Setter.Parameters;
            }
        }
        public string Name { get; private set; }

        public object Target
        {
            get { return _target; }
        }


        public Uri Url { get { return new Uri(this.Parent.Url + this.Fragment + "/", UriKind.Relative); } }
        public void SetValue(object value)
        {
            Value = value;
        }

        public Resource GetResource()
        {
            return ResourceFactory.Instance.Create(Value, this.Parent, Name);
        }

        public string TypeName { get { return "NodeProperty"; } }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            return Setter.Invoke(parameterDictionary);
        }
        public string Message { get { return ""; } }

        T IInvokeable.GetAttribute<T>()
        {
            return ((Setter == null) ? null as T : Setter.GetAttribute<T>()) ?? this.Attributes.OfType<T>().SingleOrDefault();
        }

        public IEnumerable<Attribute> Attributes
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
        public IEnumerable<string> GetValidationErrorsForValue(object value)
        {
            if (Choices != null && Choices.Cast<object>().Contains(value) == false)
                yield return "Was not a valid choice";
        }

        public IEnumerable<object> ChildNodes { get { yield break; } }

        string GetDisplayName(PropertyInfo info)
        {
            var att = info.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true).OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }


        public virtual Resource GetChild(string name)
        {
            return null;
        }



        public IEnumerable<NodeProperty> NodeProperties { get { yield break; } }
        public IEnumerable<Resource> Children { get { yield break; } }

        public string Fragment { get { return Name; } }
        
    }
}