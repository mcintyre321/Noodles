using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Helpers;

namespace Noodles.Models
{

     
    public interface INode<T>
    {
    }

    public interface IResolveChild
    {
        object ResolveChild(string name);
    }
    

    public class QueryableChild : IResolveChild
    {
        private readonly MemberInfo _mi;
        private Func<IQueryable> _getGetQueryable;
        private IQueryable _queryable;
        private INode _parent;

        public IQueryable Queryable
        {
            get { return _queryable ?? (_queryable = _getGetQueryable()); }
        }
        public IQueryable<Resource> ResourceQueryable
        {
            get
            {
                return Queryable.Cast<object>()
                                .Select(o => ResourceFactory.Instance.Create(o, _parent,  o.GetType().GetProperty(Attribute.KeyName).GetValue(o).ToString()));
            }
        }

        public ChildrenAttribute Attribute { get { return _mi.Attributes().OfType<ChildrenAttribute>().SingleOrDefault(); } }
        public QueryableChild(Func<IQueryable> getQueryable, MemberInfo mi, INode parent)
        {
            _mi = mi;
            _parent = parent;
            _getGetQueryable = getQueryable;
        }

        public object ResolveChild(string name)
        {
            return Attribute.ResolveChild(_getGetQueryable, key: name);
        }
        public static QueryableChild GetChildCollection(INode parent, object target)
        {
            return ResolversAndChildGettersFromAttributedMethods(parent, target)
                .Concat(ResolversAndChildGettersFromAttributedProperties(parent, target)).SingleOrDefault();
        }
        private static IEnumerable<QueryableChild> ResolversAndChildGettersFromAttributedMethods(INode parent, object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            return target.GetType().GetMethods(bindingFlags)
                         .Select(mi => new QueryableChild(
                             () => ((IEnumerable)mi.Invoke(target, null)).AsQueryable(),
                             mi,
                             parent
                         ))
                         .Where(pair => pair.Attribute != null);

        }
        private static IEnumerable<QueryableChild> ResolversAndChildGettersFromAttributedProperties(INode parent, object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            return target.GetType().GetProperties(bindingFlags)
                         .Select(mi => new QueryableChild(
                             () => ((IEnumerable)mi.GetGetMethod().Invoke(target, null)).AsQueryable(),
                             mi,
                             parent
                         ))
                         .Where(pair => pair.Attribute != null);
        }


        
    }
    public interface INode : IHasName
    {
        IEnumerable<object> ChildNodes { get; }
        Resource GetChild(string name); 
        string DisplayName { get; }
        Uri Url { get; }
        INode Parent { get; }
        Type ValueType { get; }
        IEnumerable<Attribute> Attributes { get; }
    }

    public static class INodeExtensions
    {
        public static IEnumerable<INode> Ancestors(this INode t) { { return t.AncestorsAndSelf().Skip(1); } }
        public static IEnumerable<INode> AncestorsAndSelf(this INode t) { { return (t).Recurse(n => n.Parent); } }
        public static INode Named(this IEnumerable<INode> nodes, string name)
        {
            return nodes.SingleOrDefault(n => n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }

}