using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Helpers;
using Walkies;
namespace Noodles.Models
{
    public class Resource : INode, IInvokeable
    {

        protected Resource(object target, INode parent, string fragment)
        {
            Value = target;
            ValueType = target.GetType();
            Parent = parent;
            
            Fragment = fragment;
        }

        public object Value { get; protected set; }
        public Type ValueType { get; set; }


        bool IInvokeable.Active
        {
            get { return true; }
        }

        IEnumerable<IInvokeableParameter> IInvokeable.Parameters
        {
            get { return this.NodeProperties.Where(x => x.Setter != null); }
        }

        string IInvokeable.Name
        {
            get { return "Set"; }
        }

        string IInvokeable.DisplayName
        {
            get { return "Set"; }
        }

        object IInvokeable.Target
        {
            get { return this; }
        }

        string IInvokeable.Message
        {
            get { return ""; }
        }

        public INode GetChild(string fragment)
        {

            var method = NodeMethods.SingleOrDefault(nm => nm.Name.ToLowerInvariant() == fragment.ToLowerInvariant());
            if (method != null) return method;

            var link = Links.SingleOrDefault(nm => nm.Name.ToLowerInvariant() == fragment.ToLowerInvariant());
            if (link != null) return link.Target;

            var property = Value.NodeProperties(this).SingleOrDefault(nm => nm.Name.ToLowerInvariant() == fragment.ToLowerInvariant());
            if (property != null) return property;
            return null;
        }

        public static Resource CreateGeneric(object target, INode parent, string fragment)
        {
            var type = target.GetType();
            var nodeType = typeof(Resource<>).MakeGenericType(type);
            return (Resource)Activator.CreateInstance(nodeType, target, parent, fragment);
        }

        private string _url;


        public string DisplayName { get { return Value.GetDisplayName(); } }

        public string Url
        {
            get { return _url ?? (Parent == null ? ( "/" + this.Fragment + "/") : (Parent.Url + Fragment + "/")); }
            set { _url = value; }
        }

        bool IInvokeable.AutoSubmit
        {
            get { return false; }
        }

        Type IInvokeable.ParameterType
        {
            get { return this.GetType(); }
        }


        object IInvokeable.Invoke(IDictionary<string, object> parameterDictionary)
        {
            foreach (var key in parameterDictionary.Keys)
            {
                var p = this.NodeProperties.Single(x => x.Name == key);
                p.Invoke(new[] {parameterDictionary[key]});
            }
            return this;
        }

        object IInvokeable.Invoke(object[] parameters)
        {
            throw new NotImplementedException();
        }

        T IInvokeable.GetAttribute<T>()
        {
            return this.Attributes().OfType<T>().SingleOrDefault();
        }

        public string Fragment { get; set; }

        public int Order { get { return int.MaxValue; } }
        public IEnumerable<NodeMethod> NodeMethods { get { return Value.NodeMethods(this); } }
        public IEnumerable<NodeProperty> NodeProperties { get { return Value.NodeProperties(this); } }
        public IEnumerable<NodeLink> Links { get { return Value.NodeLinks(this, ValueType); } }
        public INode Parent { get; protected set; }

        INode INode.Parent
        {
            get { return Parent; }
        }

        public string UiHint
        {
            get { return this.Value.Attributes().OfType<UiHintAttribute>().Select(a => a.UiHint).SingleOrDefault(); }
        }


        public IEnumerable<INode> Ancestors { get { return this.AncestorsAndSelf.Skip(1); } }
        public IEnumerable<INode> AncestorsAndSelf { get { return (this).Recurse<INode>(n => n.Parent); } }
    }

    public interface INode<T>
    {
    }

    public class Resource<T> : Resource, INode<T>
    {
        public new T Target { get { return (T)base.Value; } }

        public Resource(T target, INode parent, string fragment):base(target, parent, fragment)
        {
        }
    }
}