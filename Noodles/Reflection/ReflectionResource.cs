using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
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

    public class ChildrenAttribute : Attribute 
    {
        public string KeyName { get; private set; }
        internal readonly bool _allowEnumeration;

        public ChildrenAttribute(string key, bool allowEnumeration = false)
        {
            KeyName = key;
            _allowEnumeration = allowEnumeration;
        }

        public object ResolveChild(Func<IQueryable> getChildren, string key)
        {
            return getChildren().Where(KeyName + " == @0", key).Cast<object>().SingleOrDefault();
        }
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


    public class ReflectionResource : Resource
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

        
 

        IEnumerable<IInvokeableParameter> IInvokeable.Parameters
        {
            get
            {
                return this.ChildNodes.OfType<NodeProperty>()
                    .Where(x => !x.Readonly).Select(s => (IInvokeableParameter) s);
            }
        }

        public string InvokeDisplayName { get { return "Save"; } }
        public Uri InvokeUrl { get { return Url; } }

        object IInvokeable.Target
        {
            get { return this; }
        }


        public IEnumerable<object> ChildNodes
        {
            get
            {
                return this.Value.GetNodeMethods(this).Cast<object>()
                    .Concat(this.Value.GetNodeProperties(this))
                    .Concat(new[] {QueryableChild.GetChildCollection(this, this.Value)})
                    .Where(o => o != null);
            }
        }

        public Resource GetChild(string name)
        {
            var resolvedChild = new[]{this.Value}.Concat(ChildNodes).OfType<IResolveChild>()
                .Select(c => c.ResolveChild(name))
                .Where(o => o != null)
                .Select(o => ResourceFactory.Instance.Create(o, this, name))
                    .FirstOrDefault();
            if (resolvedChild != null) return resolvedChild;

            var childResource = this.ChildNodes.OfType<Resource>()
                   .SingleOrDefault(np => np.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (childResource != null) return childResource;

            return this.ChildNodes.OfType<NodeProperty>()
                       .Where(np => np.Name.ToLowerInvariant() == name.ToLowerInvariant())
                       .Select(np => np.GetResource())
                       .SingleOrDefault();
        }


        public string DisplayName { get { return Value.GetDisplayName(); } }

        public Uri Url
        {
            get { return Parent == null ? new Uri("/", UriKind.Relative) : (new Uri(Parent.Url.ToString() + Name + "/", UriKind.Relative)); }
            set { }
        }

        Type IInvokeable.ParameterType
        {
            get { return ValueType; }
        }

        Type IInvokeable.ResultType { get { return this.GetType(); } }


        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            foreach (var key in parameterDictionary.Keys)
            {
                var p = this.ChildNodes.OfType<NodeProperty>().SingleOrDefault(x => x.Readonly == false && x.Name == key);
                if (p == null) continue;
                p.SetValue(parameterDictionary[key]);
            }

            return null;
        }


        T IInvokeable.GetAttribute<T>()
        {
            return this.Attributes().OfType<T>().SingleOrDefault();
        }

        public int Order { get { return int.MaxValue; } }

        public INode Parent { get; protected set; }

        INode INode.Parent
        {
            get { return Parent; }
        }

        public string UiHint
        {
            get { return ""; }
        }

        public Uri RootUrl { set { }}
        public IEnumerable<Attribute> Attributes { get { return this.Value.Attributes(); } }
    }

    public class ReflectionResource<T> : ReflectionResource,  Resource<T>, INode<T>
    {
        public new T Target { get { return (T)base.Value; } }

        public ReflectionResource(T target, INode parent, string name):base(target, parent, name)
        {
        }
    }
}