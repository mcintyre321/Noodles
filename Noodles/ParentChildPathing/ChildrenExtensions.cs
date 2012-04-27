using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public static class ChildrenExtensions
    {
        static ChildrenExtensions()
        {
            Rules = new List<ResolveChild>()
            {
                ResolveChildFromIHasChildren,
                ResolveChildFromIEnumerable,
                ResolveChildFromMetaProps_Children,

            };
        }

        public static List<ResolveChild> Rules;
        public static object GetChild(this object o, string name)
        {
            return Rules.Select(r => r(o, name)).FirstOrDefault(child => child != null);
        }

        public static ResolveChild ResolveChildFromMetaProps_Children = (o, name) => WeakTableChildStore.GetChildForObject(o, name);
        
        public static ResolveChild ResolveChildFromIHasChildren = (object o, string name) =>
        {
            if (o is IHasChildren)
            {
                return ((IHasChildren) o).GetChild(name);
            }
            return null;
        };

        public static ResolveChild ResolveChildFromIEnumerable = (o, name) =>
        {
            if (o is IEnumerable)
            {
                return ((IEnumerable) o).Cast<object>().FirstOrDefault(x => x.Name() == name);
            }
            return null;
        };

    }
}