using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Noodles.Helpers;

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


    public class ReflectionResource : Resource, INode, IInvokeable
    {

        protected ReflectionResource(object target, INode parent, string name)
        {
            Value = target;
            ValueType = target.GetType();
            Parent = parent;
            
            Name = name;
        }

        public string Name { get; set; }

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

        object IInvokeable.Target
        {
            get { return this; }
        }

        string IInvokeable.Message
        {
            get { return ""; }
        }

        public INode GetChild(string name)
        {

            var method = NodeMethods.SingleOrDefault(nm => nm.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (method != null) return method;

            var link = NodeLinks.SingleOrDefault(nm => nm.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (link != null) return link.Target;

            var property = Value.GetNodeProperties(this).SingleOrDefault(nm => nm.Name.ToLowerInvariant() == name.ToLowerInvariant());
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


        public string DisplayName { get { return Value.GetDisplayName(); } }

        public Uri Url
        {
            //_url should have been set via RootUrl if .Parent is null (i.e. this is a root object)
            get { return _url ?? (new Uri(Parent.Url.ToString() + Name + "/", UriKind.Relative)); }
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

        public int Order { get { return int.MaxValue; } }
        public IEnumerable<NodeMethod> NodeMethods { get { return Value.GetNodeMethods(this); } }
        public IEnumerable<NodeProperty> NodeProperties { get { return Value.GetNodeProperties(this); } }
        public IEnumerable<NodeLink> NodeLinks { get { return Value.NodeLinks(this, ValueType); } }
        public INode Parent { get; protected set; }

        INode INode.Parent
        {
            get { return Parent; }
        }

        public string UiHint
        {
            get { return ""; }
        }

        public Uri RootUrl { set { _url = value; }}
    }

    public class ReflectionResource<T> : ReflectionResource,  Resource<T>, INode<T>
    {
        public new T Target { get { return (T)base.Value; } }

        public ReflectionResource(T target, INode parent, string name):base(target, parent, name)
        {
        }
    }
}