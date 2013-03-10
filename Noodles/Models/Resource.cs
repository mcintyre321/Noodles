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
            Value = target;
            ValueType = target.GetType();
            Parent = parent;
            var fragment = target.GetFragment();
            if (fragment == null)
            {
                if (parent != null)
                {
                    fragment = Guid.NewGuid().ToString();
                    target.SetFragment(fragment);
                }
                else
                {
                    fragment = target.GetUrlRoot().Trim('/');
                }
            }
            Fragment = fragment;
        }

        public object Value { get; protected set; }
        public Type ValueType { get; set; }

        public Type Type { get { return this.GetType(); } }
        public string DisplayName
        {
            get { return Value.GetDisplayName(); }
        }

        public INode GetChild(string fragment)
        {
            var child = Walkies.WalkExtension.Child(Value, fragment);
            if (child != null) return CreateGeneric(child, this);

            var method = Value.NodeMethods(this).SingleOrDefault(nm => nm.Name.ToLowerInvariant() == fragment.ToLowerInvariant());
            if (method != null) return method;

            var property = Value.NodeProperties(this).SingleOrDefault(nm => nm.Name.ToLowerInvariant() == fragment.ToLowerInvariant());
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
            get { return _url ?? (Parent == null ? ( "/" + this.Fragment + "/") : (Parent.Url + Fragment + "/")); }
            set { _url = value; }
        }

        public string Fragment { get; set; }

        public int Order { get { return int.MaxValue; } }
        public IEnumerable<NodeMethod> NodeMethods { get { return Value.NodeMethods(this); } }
        public IEnumerable<INode> NodeProperties { get { return Value.NodeProperties(this); } }
        public INode Parent { get; protected set; }

        public string UiHint
        {
            get { return this.Value.Attributes().OfType<UiHintAttribute>().Select(a => a.UiHint).SingleOrDefault(); }
        }

        public string TypeName { get { return this.Type.FullName; } }

        public IEnumerable<INode> Ancestors { get { return this.AncestorsAndSelf.Skip(1); } }
        public IEnumerable<INode> AncestorsAndSelf { get { return (this).Recurse<INode>(n => n.Parent); } }
        public IEnumerable<INode> Children { get { return (this.Value.KnownChildren().Select(c => Resource.CreateGeneric(c, this))); } }
    }

    public interface INode<T>
    {
    }

    public class Resource<T> : Resource, INode<T>
    {
        public new T Target { get { return (T)base.Value; } }

        public Resource(T target, INode parent):base(target, parent)
        {
        }
    }
}