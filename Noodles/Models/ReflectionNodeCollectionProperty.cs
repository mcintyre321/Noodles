using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DelegateQueryable;
using Noodles.Helpers;
using System.Linq.Dynamic;
namespace Noodles.Models
{
    [DisplayName("{DisplayName}")]
    public class ReflectionNodeCollectionProperty:  NodeCollectionProperty  
    {
        private readonly INode _parent;
        private readonly object _target;
        private readonly PropertyInfo _info;
        private readonly Type _collectionItemType;

        public ReflectionNodeCollectionProperty(INode parent, object target, PropertyInfo info, Type collectionItemType)
        {
            _parent = parent;
            _target = target;
            _info = info;
            _collectionItemType = collectionItemType;
        }

        public IEnumerable<INode> ChildNodes { get { return this.GetNodeMethods(this).Concat(this.GetNodeMethods(this)); } }

        public Resource GetChild(string name)
        {
            var slugPropertyName = SlugAttribute.GetSlugPropertyName(_collectionItemType);
            if (string.IsNullOrWhiteSpace(slugPropertyName))
            {
                int index = 0;
                if (int.TryParse(name, out index))
                {
                    return Query(index, 1).Items.SingleOrDefault();
                }
            }
            else
            {
                return ((IEnumerable)Value).AsQueryable()
                                            .Where(slugPropertyName + " == @0", name)
                                            .Cast<object>()
                                            .Where(o => o != null)
                                            .Select(o => ResourceFactory.Instance.Create(o, this, name))
                                            .SingleOrDefault();
            }
            return null;
        }

        protected IEnumerable Value { get { return (IEnumerable) _info.GetValue(_target); } }

        public string DisplayName { get { return _info.Name.Sentencise(); } }
        public Uri Url { get{return new Uri(_parent.Url + this.Name + "/", UriKind.Relative);} }
        public INode Parent { get { return _parent; } }
        public Type ValueType { get { return _collectionItemType; } }

        public IEnumerable<Attribute> Attributes
        {
            get
            {
                var atts = _info.Attributes();
                var getter = _info.GetGetMethod();
                if (getter != null) atts = atts.Concat(getter.Attributes());
                var setter = _info.GetSetMethod();
                if (setter != null) atts = atts.Concat(setter.Attributes());
                return atts;
            }
        }


        [Show]
        public QueryPage Query(int skip, int take)
        {
            return new QueryPage(skip, take, Items, this);
        }

        IQueryable<object> Items
        {
            get
            {
                var queryable = Value as IQueryable<object>;
                if (queryable != null)
                {
                    var delegateQueryable = queryable.ToDelegateQueryable();
                    return delegateQueryable;
                }
                return ((IEnumerable)Value).AsQueryable().Cast<object>();
            }
        }


        public string Name { get { return _info.Name; } }


        public IEnumerable<IInvokeableParameter> Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public string InvokeDisplayName
        {
            get { throw new NotImplementedException(); }
        }

        public Uri InvokeUrl
        {
            get { throw new NotImplementedException(); }
        }

        public object Target
        {
            get { throw new NotImplementedException(); }
        }

        public Type ParameterType
        {
            get { throw new NotImplementedException(); }
        }

        public Type ResultType
        {
            get { throw new NotImplementedException(); }
        }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            throw new NotImplementedException();
        }

        public T GetAttribute<T>() where T : Attribute
        {
            return this.Attributes.OfType<T>().SingleOrDefault();
        }
    }
}