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
    public class NodeCollectionProperty : NodeProperty
    {
        public NodeCollectionProperty(INode parent, object target, PropertyInfo info)
            : base(parent, target, info)
        {
        }

        public override INode GetChild(string fragment)
        {
            int index = 0;
            if (int.TryParse(fragment, out index))
            {
                return Query(index, 1).Items.SingleOrDefault();
            }
            return base.GetChild(fragment);
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
            get { return this.NodeMethods(this); }
        }

    }
}