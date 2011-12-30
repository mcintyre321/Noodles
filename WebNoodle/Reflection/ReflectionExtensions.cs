using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebNoodle.Reflection
{
    public static class ReflectionExtensions
    {
        static ReflectionExtensions()
        {
            NodeMethodFilters = new List<Func<object, MethodInfo, bool>>()
            {
            };
        }

        public static IList<Func<object, MethodInfo, bool>> NodeMethodFilters { get; private set; }

        public static IEnumerable<IObjectMethod> GetNodeMethodInfos(this INode o, object behaviour = null)
        {
            var methods = o.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            return methods
                .Where(mi => !mi.Name.StartsWith("get_"))
                .Where(mi => mi.DeclaringType != typeof(System.Object))
                .Where(mi => !mi.Name.EndsWith("_value", StringComparison.InvariantCultureIgnoreCase)) //choices should be hidden
                .Where(mi => !mi.Name.EndsWith("_values", StringComparison.InvariantCultureIgnoreCase)) //choices should be hidden
                .Where(mi => !mi.Name.EndsWith("_choices", StringComparison.InvariantCultureIgnoreCase)) //choices should be hidden
                .Where(mi => !mi.Name.EndsWith("_suggestions", StringComparison.InvariantCultureIgnoreCase)) //suggestions should be hidden
                .Where(mi => mi.Name != "NodeMethods")
                .Where(mi => mi.Name != "GetBehavioursFor")
                .Where(mi => mi.Name != "Children")
                .Where(mi => mi.Name != "GetEnumerator")
                .Where(mi => !mi.Name.StartsWith("_"))
                .Where(mi => NodeMethodFilters.All(mf => mf(o, mi)))
                .Select(mi => new ObjectMethod(o, behaviour ?? o, mi))
                .ToArray();
        }
    }
}