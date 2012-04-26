using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Helpers;

namespace Noodles
{
    public static class ParentExtensions
    {
        public static object Parent(this object child)
        {
            return ParentRules.Select(r => r(child)).FirstOrDefault(parent => parent != null);
        }
        
        public static IEnumerable<object> Ancestors(this object obj)
        {
            return obj.Recurse(o => o.Parent()).Skip(1);
        }
        public static IEnumerable<object> SelfAndAncestors(this object obj)
        {
            return obj.Recurse(o => o.Parent());
        }
        public static ResolveParent GetParentFromIHasParent = o =>
        {
            var hasParent = o as IHasParent<object>;
            if (hasParent != null)
            {
                return (hasParent).Parent;
            }
            return null;
        };
       
        static ParentExtensions()
        {
            ParentRules = new List<ResolveParent>()
            {
                GetParentFromIHasParent,
                ParentAttribute.ResolveParentFromAttributedPropertyRule,
                ParentAttribute.ResolveParentFromAttributedFieldRule,
                WeakTableChildStore.GetParentFromMetaProps
            };
        }
        public static List<ResolveParent> ParentRules;

       
    }
}