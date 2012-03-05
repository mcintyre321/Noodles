using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public static class ImplicitConversionMethodHelper
    {
        static ConcurrentDictionary<Type, ConcurrentDictionary<Type, MethodInfo>> cache = new
            ConcurrentDictionary<Type, ConcurrentDictionary<Type, MethodInfo>>();
        public static MethodInfo ImplicitConversionMethod(Type from, Type to)
        {
            var inner = cache.GetOrAdd(to, (t) => new ConcurrentDictionary<Type, MethodInfo>());
            return inner.GetOrAdd(from, t => UncachedGetImplicitConversionMethod(from, to));
        }


        public static MethodInfo UncachedGetImplicitConversionMethod(Type from, Type to)
        {
            return to.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static)
                .Where(x => x.Name == "op_Implicit")
                .Where(x => to.IsAssignableFrom(x.ReturnType))
                .Where(x => x.GetParameters().Single().ParameterType == from)
                .FirstOrDefault();
        }

    }
}