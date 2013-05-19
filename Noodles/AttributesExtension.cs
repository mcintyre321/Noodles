using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Noodles
{
    static class AttributesExtension
    {
        static ConcurrentDictionary<Type, IEnumerable<Attribute>> typeLookup = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();
        static ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>> piLookup = new ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>>();
        static ConcurrentDictionary<MethodInfo, IEnumerable<Attribute>> miLookup = new ConcurrentDictionary<MethodInfo, IEnumerable<Attribute>>();
        static ConcurrentDictionary<FieldInfo, IEnumerable<Attribute>> fiLookup = new ConcurrentDictionary<FieldInfo, IEnumerable<Attribute>>();
        public static IEnumerable<Attribute> Attributes(this object o)
        {
            return o.GetType().Attributes();
        }
      
        public static IEnumerable<Attribute> Attributes(this PropertyInfo propertyInfo)
        {
            return piLookup.GetOrAdd(propertyInfo, p => p.GetCustomAttributes(true).Cast<Attribute>());
        }
        public static IEnumerable<Attribute> Attributes(this MethodInfo methodInfo)
        {
            return miLookup.GetOrAdd(methodInfo, m => m.GetCustomAttributes(true).Cast<Attribute>());
        }
        public static IEnumerable<Attribute> Attributes(this FieldInfo methodInfo)
        {
            return fiLookup.GetOrAdd(methodInfo, m => m.GetCustomAttributes(true).Cast<Attribute>());
        }

        public static IEnumerable<Attribute> Attributes(this Type type)
        {
            return typeLookup.GetOrAdd(type, (t) => t.GetCustomAttributes(true).Cast<Attribute>());
        }
    }
}
