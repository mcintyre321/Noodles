using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace WebNoodle.Reflection
{
    public static class ImplicitConversionMethodHelper
    {
        static readonly ConcurrentDictionary<Type, MethodInfo> cache = new ConcurrentDictionary<Type, MethodInfo>();
        public static MethodInfo ImplicitStringConversionMethod(this Type modelType)
        {
            return cache.GetOrAdd(modelType, UncachedGetImplicitConversionMethod);
        }

        private static MethodInfo UncachedGetImplicitConversionMethod(Type modelType)
        {
            return modelType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static)
                .Where(x => x.Name == "op_Implicit")
                .Where(x => modelType.IsAssignableFrom(x.ReturnType))
                .Where(x => x.GetParameters().Single().ParameterType == typeof (string))
                .FirstOrDefault();
        }

    }
}