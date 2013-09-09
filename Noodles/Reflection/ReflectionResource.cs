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

    public class GetChildByAttribute : Attribute, IResolveChildren
    {
        private readonly string _searchField;

        public GetChildByAttribute(string searchField)
        {
            _searchField = searchField;
        }

        public object ResolveChild(Func<IQueryable> getChildren, string key)
        {
            return getChildren().Where(_searchField + " == @0", key).Cast<object>().SingleOrDefault();
        }
    }

    public interface IResolveChildren
    {
        object ResolveChild(Func<IQueryable> getChildren, string key);
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
        private static IEnumerable<Tuple<Func<IQueryable>, IResolveChildren>> ResolversAndChildGettersFromAttributedMethods(object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            return target.GetType().GetMethods(bindingFlags)
                         .Select(mi => new Tuple<Func<IQueryable>, IResolveChildren>(
                             () => ((IEnumerable)mi.Invoke(target, null)).AsQueryable(),
                             mi.Attributes().OfType<IResolveChildren>().SingleOrDefault()
                         ))
                         .Where(pair => pair.Item2 != null);

        }
        private static IEnumerable<Tuple<Func<IQueryable>, IResolveChildren>> ResolversAndChildGettersFromAttributedProperties(object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            return target.GetType().GetProperties(bindingFlags)
                         .Select(mi => new Tuple<Func<IQueryable>, IResolveChildren>(  
                             () => ((IEnumerable)mi.GetGetMethod().Invoke(target, null)).AsQueryable(), 
                             mi.Attributes().OfType<IResolveChildren>().SingleOrDefault()
                         ))
                         .Where(pair => pair.Item2 != null);
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


        public IEnumerable<INode> ChildNodes { get { return this.Value.GetNodeMethods(this).Concat(this.Value.GetNodeProperties(this)); } }

        public Resource GetChild(string name)
        {
            var childFromMarkedEnumerables =
                ResolversAndChildGettersFromAttributedMethods(this.Value)
                    .Concat(ResolversAndChildGettersFromAttributedProperties(this.Value))
                    .Select(pair => pair.Item2.ResolveChild(pair.Item1, name))
                    .Select(o => ResourceFactory.Instance.Create(o, this, name))
                    .FirstOrDefault();
            if (childFromMarkedEnumerables != null) return childFromMarkedEnumerables;

            //var method = NodeMethods.SingleOrDefault(nm => nm.Name.ToLowerInvariant() == name.ToLowerInvariant());
            //if (method != null) return method;
            var childMethod = this.ChildNodes.OfType<NodeMethod>()
                   .SingleOrDefault(np => np.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (childMethod != null) return childMethod;

            var childProperty = this.ChildNodes.OfType<NodeProperty>()
                    .SingleOrDefault(np => np.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (childProperty != null) return ResourceFactory.Instance.Create(childProperty.Value, this, childProperty.Name);

            var childCollectionProperty = this.ChildNodes.OfType<NodeCollectionProperty>()
                                .SingleOrDefault(np => np.Name.ToLowerInvariant() == name.ToLowerInvariant());
            if (childCollectionProperty != null) return childCollectionProperty;


            return null;
        }

      

        private Uri _url;


        public string DisplayName { get { return Value.GetDisplayName(); } }

        public Uri Url
        {
            get { return Parent == null ? new Uri("/", UriKind.Relative) : (new Uri(Parent.Url.ToString() + Name + "/", UriKind.Relative)); }
            set { _url = value; }
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

        public Uri RootUrl { set { _url = value; }}
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