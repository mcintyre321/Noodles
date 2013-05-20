using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DelegateQueryable;
using Noodles.Helpers;

namespace Noodles.Models
{
    [DisplayName("{DisplayName}")]
    public class NodeCollectionProperty<TParent> : ReflectionNodeProperty<TParent>, NodeCollectionProperty, NodeProperty where TParent : INode
    {
        public NodeCollectionProperty(TParent parent, object target, PropertyInfo info)
            : base(parent, target, info)
        {
        }

        INode INode.GetChild(string name)
        {
            int index = 0;
            if (int.TryParse(name, out index))
            {
                return Query(index, 1).Items.SingleOrDefault();
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