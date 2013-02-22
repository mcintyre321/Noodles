using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DelegateQueryable;
using Noodles.Helpers;
using Noodles.Requests;
using Walkies;
namespace Noodles
{
    [DisplayName("{DisplayName}")]
    public class NodeProperty : IInvokeable, INode
    {
        private readonly object _target;
        private readonly PropertyInfo _info;
        private object _value;

        public NodeProperty(Resource parent, object target, PropertyInfo info)
        {
            _target = target;
            _info = info;
            Value = info.GetValue(target, null);
            PropertyType = info.PropertyType;
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
        public Type PropertyType { get; private set; }
        public object Value
        {
            get
            {
                return _value;
            }
            private set { _value = value; }
        }
        public int Order
        {
            get
            {
                return this._info.Attributes().OfType<ShowAttribute>().Select(a => a.UiOrder as int?).SingleOrDefault() ?? int.MaxValue;
            }
        }

        public bool Active { get { return !Readonly; } }
        public IEnumerable<NodeMethodParameter> Parameters { get
        {
            if (Setter == null) return Enumerable.Empty<NodeMethodParameter>();
            return Setter.Parameters.Then(p => p.DisplayName = this.DisplayName);
        }}
        public string Name { get; private set; }

        public object Target
        {
            get { return _target; }
        }

        public string Url { get { return this.Parent.Url + this.Fragment + "/"; } }
        public INode Parent { get; set; }
        public string UiHint { get { return _info.Attributes().OfType<ShowAttribute>().Select(a => a.UiHint).SingleOrDefault(); } }
        public bool AutoSubmit { get { return false; } }
        public Type SignatureType { get { return Setter.SignatureType; } }

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
            return ((Setter == null) ? null as T : Setter.GetAttribute<T>() ) ?? this.CustomAttributes.OfType<T>().SingleOrDefault();
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
        string GetDisplayName(FieldInfo info)
        {
            var att = info.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true).OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }
         
        public INode GetChild(string fragment)
        {
            var items = Items ?? Enumerable.Empty<INode>();
            return items.FirstOrDefault(n => n.Fragment.ToLowerInvariant() == fragment.ToLowerInvariant());
        }

        public IQueryable<Resource> Items
        {
            get
            {
                if (Value is IEnumerable && this.PropertyType != typeof(string))
                {
                    var queryable = Value as IQueryable<object>;
                    if (queryable != null)
                    {
                        var delegateQueryable = queryable.ToDelegateQueryable();
                        return delegateQueryable.AsQueryable().Select(item => Resource.CreateGeneric(item, this)).Cast<Resource>();
                    }
                    return ((IEnumerable)Value).AsQueryable().Cast<object>().Select(r => Resource.CreateGeneric(r, this));
                }
                return null;
            }
        }

        
        public string Fragment { get { return Name; }}
    }
}