using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebNoodle.Reflection
{
    public static class GetMethodInfosViaReflectionExtension
    {
        static GetMethodInfosViaReflectionExtension()
        {
            MethodFilters = new List<Func<object, MethodInfo, bool>>()
            {
            };
        }

        public static IList<Func<object, MethodInfo, bool>> MethodFilters { get; set; }

        public static IEnumerable<IObjectMethod> GetNodeMethodInfos<T>(this T o) where T : IBehaviour
        {
            var methods = o.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            return methods
                .Where(mi => !mi.Name.StartsWith("get_"))
                .Where(mi => mi.DeclaringType != typeof(System.Object))
                .Where(mi => !mi.Name.EndsWith("_choices", StringComparison.InvariantCultureIgnoreCase)) //choices should be hidden
                .Where(mi => !mi.Name.EndsWith("_suggestions", StringComparison.InvariantCultureIgnoreCase)) //suggestions should be hidden
                .Where(mi => mi.Name != "NodeMethods")
                .Where(mi => mi.Name != "GetBehavioursFor")
                .Where(mi => mi.Name != "Children")
                .Where(mi => mi.Name != "GetEnumerator")
                .Where(mi => mi.Name.StartsWith("_"))
                .Where(mi => MethodFilters.All(mf => mf(o, mi)))
                .Select(mi => new ObjectMethod(o, mi))
                .ToArray();
        }
    }
}