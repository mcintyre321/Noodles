using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace WebNoodle.Reflection
{
    public static class ImplicitConversionMethodHelper
    {
        
        public static MethodInfo ImplicitConversionMethod(Type from, Type to)
        {
            return UncachedGetImplicitConversionMethod(from, to);
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