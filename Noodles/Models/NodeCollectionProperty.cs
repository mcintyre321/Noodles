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
            var items = Items ?? Enumerable.Empty<INode>();
            return items.FirstOrDefault(n => n.Fragment.ToLowerInvariant() == fragment.ToLowerInvariant())
                   ?? base.GetChild(fragment);
        }



        [Show]
        public QueryPage Query(int skip, int take)
        {
            return new QueryPage(skip, take, Items.Skip(skip).Take(take));
        }

        public IQueryable<Resource> Items
        {
            get
            {
                if (Value is IEnumerable && this.ValueType != typeof(string))
                {
                    var queryable = Value as IQueryable<object>;
                    if (queryable != null)
                    {
                        var delegateQueryable = queryable.ToDelegateQueryable();
                        return delegateQueryable.AsQueryable()
                            .Select((i) => Resource.CreateGeneric(i, this, Guid.NewGuid().ToString())).Cast<Resource>();
                    }
                    return ((IEnumerable)Value).AsQueryable().Cast<object>().Select((r, i) => Resource.CreateGeneric(r, this, i.ToString()));
                }
                return null;
            }
        }
        public override IEnumerable<NodeMethod> NodeMethods
        {
            get { return this.NodeMethods(this); }
        }

    }
}