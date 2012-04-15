using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public interface IHasParent<out T>
    {
        T Parent { get; }
    }
    public delegate object ResolveParent(object child);

    public static class ParentExtension
    {
        public static object Parent(this object child)
        {
            return ParentRules.Select(r => r(child)).FirstOrDefault(parent => parent != null);
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

        static ParentExtension()
        {
            ParentRules = new List<ResolveParent>()
            {
                GetParentFromIHasParent
            };
        }

        public static List<ResolveParent> ParentRules;

    }
}