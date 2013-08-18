using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Noodles.Helpers;

namespace Noodles.Models
{
    public class ResourceFactory
    {
        public List<Func<object, INode, string, Resource>> Rules = new List<Func<object, INode, string, Resource>>()
        {
            CreateReflectionResource
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

        static Resource CreateReflectionResource(object target, INode parent, string fragment)
        {
            if (target == null) throw new NullReferenceException("Null resource at'" + parent.Url + "/" + fragment + "' ");
            var type = target.GetType();
            var nodeType = typeof(ReflectionResource<>).MakeGenericType(type);
            return (ReflectionResource)Activator.CreateInstance(nodeType, target, parent, fragment);
        }

    }

    public class GetChildAttribute : Attribute, IChildInfo
    {
        public string UiHint { get; set; }
    }

    public interface IChildInfo
    {
        string UiHint { get; }
    }

    /// <summary>
    /// If a property  is marked as a Slug, it will be used to build the url instead of the collection index
    /// </summary>
    public class SlugAttribute : Attribute
    {
        public static string GetSlug(object o)
        {
            var pi = GetSlugProperty(o.GetType());
            if (pi == null) return null;
            return pi.GetValue(o) as string;
        }

        public static string GetSlugPropertyName(Type valueType)
        {
            var slugProperty = GetSlugProperty(valueType);
            if (slugProperty == null) return null;
            return slugProperty.Name;
        }

        public static PropertyInfo GetSlugProperty(Type valueType)
        {
            return valueType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault(pi => pi.Attributes().OfType<SlugAttribute>().Any());
        }

    }


    public class ReflectionResource : Resource, INode, IInvokeable
    {
        public static readonly List<Func<object, string, Tuple<object, IChildInfo>>> GetChildRules = new List<Func<object, string, Tuple<object, IChildInfo>>>()
        {
            (GetChildFromAttributedMethods)
        };

        private static Tuple<object, IChildInfo> GetChildFromAttributedMethods(object target, string slug)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            var methodInfos = target.GetType().GetMethods(bindingFlags)
                .Select(mi => new { mi, GetChildAttribute = mi.Attributes().OfType<GetChildAttribute>().SingleOrDefault() })
                    .Where(pair => pair.GetChildAttribute != null);
            var childFromAttributedMethods = methodInfos.Select(m => Tuple.Create(m.mi.Invoke(target, new object[] { slug }), (IChildInfo) m.GetChildAttribute))
                .FirstOrDefault(o => o.Item1 != null);
            return childFromAttributedMethods;
        }

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

        public string InvokeDisplayName { get { return "Save"; } }
        public Uri InvokeUrl { get { return Url; } }

        object IInvokeable.Target
        {
            get { return this; }
        }

        string IInvokeable.Message
        {
            get { return ""; }
        }

        public IEnumerable<INode> ChildNodes { get { return this.NodeLinks.Cast<INode>().Concat(this.NodeMethods).Concat(this.NodeProperties); } }

        public INode GetChild(string name)
        {
            var child = GetChildRules.Select(r => r(Value, name)).Where(c => c != null)
                                        .Select(o => ResourceFactory.Instance.Create(o.Item1, this, name))
                                        .FirstOrDefault();
            if (child != null) return child;

            var method = NodeMethods.SingleOrDefault(nm => nm.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (method != null) return method;

            var link = NodeLinks.SingleOrDefault(nm => nm.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (link != null) return link.Target;

            var property = Value.GetNodeProperties(this).SingleOrDefault(nm => nm.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (property != null) return property;

            return null;
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
            return null;
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
        public IEnumerable<Attribute> CustomAttributes { get { return this.Value.Attributes(); } }
    }

    public class ReflectionResource<T> : ReflectionResource,  Resource<T>, INode<T>
    {
        public new T Target { get { return (T)base.Value; } }

        public ReflectionResource(T target, INode parent, string name):base(target, parent, name)
        {
        }
    }
}