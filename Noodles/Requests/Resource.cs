using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Helpers;

namespace Noodles.Requests
{
    public class Resource : INode
    {
        public object Target { get; protected set; }

        public string DisplayName
        {
            get { return Target.GetDisplayName(); }
        }

        public INode GetChild(string fragment)
        {
            var child = Walkies.WalkExtension.Child(Target, fragment);
            if (child != null) return CreateGeneric(child, this);
        
            var method = Target.NodeMethods(this).SingleOrDefault(nm => nm.Name.ToLowerInvariant() == fragment.ToLowerInvariant());
            if (method != null) return method;

            var property = Target.NodeProperties(this).SingleOrDefault(nm => nm.Name.ToLowerInvariant() == fragment.ToLowerInvariant());
            if (property != null) return property;
            return null;
        }

        public static Resource CreateGeneric(object target, Resource parent)
        {
            var type = target.GetType();
            var nodeType = typeof(Resource<>).MakeGenericType(type);
            return (Resource)Activator.CreateInstance(nodeType, target, parent);
        }

        private string _url;
        public string Url
        {
            get { return _url ?? (Parent.Url + Fragment + "/"); }
            set { _url = value; }
        }

        public string Fragment { get; set; }

        public IEnumerable<NodeMethod> NodeMethods { get { return Target.NodeMethods(this); } }
        public IEnumerable<NodeProperty> NodeProperties { get { return Target.NodeProperties(this); } }

        public INode Parent { get; protected set; }
        public IEnumerable<Resource> ChildResources { get { yield break; } }
        public IEnumerable<INode> Ancestors { get { return this.AncestorsAndSelf.Skip(1); } }
        public IEnumerable<INode> AncestorsAndSelf { get { return (this).Recurse<INode>(n => n.Parent); } }
    }

    public interface INode
    {
        INode GetChild(string fragment);
        string Fragment { get; }
        string DisplayName { get; }
        string Url { get; }
        INode Parent { get; }
    }
    public interface INode<T>
    {
    }

    public class Resource<T> : Resource, INode<T>
    {
        public new T Target { get { return (T)base.Target; } }

        public Resource(T target, Resource parent)
        {
            base.Target = target;
            base.Parent = parent;
        }
    }
}