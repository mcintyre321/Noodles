using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Noodles.Helpers;
using Walkies;
namespace Noodles.Models
{
    public class ResourceFactory
    {
        public List<Func<object, INode, string, Resource>> Rules = new List<Func<object, INode, string, Resource>>()
        {
            ReflectionResource.CreateGeneric
        };
        private static Lazy<ResourceFactory> _instance = new Lazy<ResourceFactory>(() => new ResourceFactory());

        public static ResourceFactory Instance
        {
            get { return _instance.Value; }
        }

        public Resource Create(object target, INode parent, string fragment)
        {
            return Rules.Select(r => r(target, parent, fragment)).FirstOrDefault();
        }

    } 

    public interface Resource : INode, IInvokeable
    {
        object Value { get; }
        Type ValueType { get; }

        IEnumerable<NodeProperty> NodeProperties { get; }
        IEnumerable<NodeMethod> NodeMethods { get; }
        IEnumerable<NodeLink> Links { get; }

        IEnumerable<INode> Ancestors { get; }
        IEnumerable<INode> AncestorsAndSelf { get; }
        Uri RootUrl { set; }
    }

    public interface Resource<T> : Resource, INode<T>
    {
        new T Target { get; } 
    }

    public class ReflectionResource : Resource, INode, IInvokeable
    {

        protected ReflectionResource(object target, INode parent, string fragment)
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
            get { return this.NodeProperties.Where(x => !x.Readonly).Select(s => (IInvokeableParameter) s); }
        }

        string IInvokeable.Name
        {
            get { return "Update"; }
        }

        string IInvokeable.DisplayName
        {
            get { return "Update"; }
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
            var nodeType = typeof(ReflectionResource<>).MakeGenericType(type);
            return (ReflectionResource)Activator.CreateInstance(nodeType, target, parent, fragment);
        }

        private Uri _url;


        string INode.DisplayName { get { return Value.GetDisplayName(); } }

        public Uri Url
        {
            get { return _url ?? (Parent == null ? new Uri("/" + this.Fragment + "/", UriKind.Relative) : new Uri(Parent.Url.ToString() + Fragment + "/", UriKind.Relative)); }
            set { _url = value; }
        }

        bool IInvokeable.AutoSubmit
        {
            get { return false; }
        }

        Type IInvokeable.ParameterType
        {
            get { return ValueType; }
        }

        Type IInvokeable.ResultType { get { return this.GetType(); } }


        object IInvokeable.Invoke(IDictionary<string, object> parameterDictionary)
        {
            foreach (var key in parameterDictionary.Keys)
            {
                var p = this.NodeProperties.SingleOrDefault(x => x.Readonly == false && x.Name == key);
                if (p == null) continue;
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
            get { return ""; }
        }


        public IEnumerable<INode> Ancestors { get { return this.AncestorsAndSelf.Skip(1); } }
        public IEnumerable<INode> AncestorsAndSelf { get { return (this).Recurse<INode>(n => n.Parent); } }
        public Uri RootUrl { set { _url = value; }}
    }

    public class ReflectionResource<T> : ReflectionResource,  Resource<T>, INode<T>
    {
        public new T Target { get { return (T)base.Value; } }

        public ReflectionResource(T target, INode parent, string fragment):base(target, parent, fragment)
        {
        }
    }
}