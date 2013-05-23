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
    public class NodeCollectionProperty<TParent> : ReflectionNodeProperty<TParent>, NodeCollectionProperty, NodeProperty where TParent : INode
    {
        private readonly Type _collectionItemType;

        public NodeCollectionProperty(TParent parent, object target, PropertyInfo info, Type collectionItemType)
            : base(parent, target, info)
        {
            _collectionItemType = collectionItemType;
        }

        INode INode.GetChild(string name)
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
                return ((IEnumerable) Value).AsQueryable()
                                            .Where(slugPropertyName + " == @0", name)
                                            .Cast<object>()
                                            .Where(o => o != null)
                                            .Select(o => ResourceFactory.Instance.Create(o, this, name))
                                            .SingleOrDefault();
            }
            return base.GetChild(name);
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
                return ((IEnumerable) Value).AsQueryable().Cast<object>();
            }
        }
        public override IEnumerable<NodeMethod> NodeMethods
        {
            get { return this.GetNodeMethods(this); }
        }
    }
}