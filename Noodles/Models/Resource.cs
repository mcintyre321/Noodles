using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Helpers;
using Walkies;
namespace Noodles.Models
{
    public class Resource : INode
    {

        protected Resource(object target, INode parent)
        {
            Target = target;
            Parent = parent;
            var fragment = target.GetFragment();
            if (fragment == null)
            {
                fragment = Guid.NewGuid().ToString();
                target.SetFragment(fragment);
            }
            Fragment = fragment;
        }

        public object Target { get; protected set; }
        public Type Type { get { return Target.GetType(); } }
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

        public static Resource CreateGeneric(object target, INode parent)
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

        public string UiHint
        {
            get { return this.Target.Attributes().OfType<UiHintAttribute>().Select(a => a.UiHint).SingleOrDefault(); }
        }

        public IEnumerable<INode> Ancestors { get { return this.AncestorsAndSelf.Skip(1); } }
        public IEnumerable<INode> AncestorsAndSelf { get { return (this).Recurse<INode>(n => n.Parent); } }
        public IEnumerable<INode> Children { get { return (this.Target.KnownChildren().Select(c => Resource.CreateGeneric(c, this))); } }
    }

    public interface INode<T>
    {
    }

    public class Resource<T> : Resource, INode<T>
    {
        public new T Target { get { return (T)base.Target; } }

        public Resource(T target, INode parent):base(target, parent)
        {
        }
    }
}