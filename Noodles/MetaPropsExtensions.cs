using System.Collections;
using System.Runtime.CompilerServices;

namespace Noodles
{
    public static class MetaPropsExtensions
    {
        private static readonly ConditionalWeakTable<object, Hashtable> Props =
            new ConditionalWeakTable<object, Hashtable>();

        public static Hashtable Meta(this object key)
        {
            return Props.GetOrCreateValue(key);
        }
    }
}