using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Noodles
{
    static class MembersExtension
    {
        public static IEnumerable<MethodInfo> AllMethodInfos(this Type t)
        {
            return t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        }
        public static IEnumerable<FieldInfo> AllFieldInfos(this Type t)
        {
            return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        }
        public static IEnumerable<PropertyInfo> AllPropertyInfos(this Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        } 

    }
    static class ReflectionHelpersExtensions
    {
        static ConcurrentDictionary<Type, IEnumerable<Attribute>> typeLookup = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();
        static ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>> piLookup = new ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>>();
        static ConcurrentDictionary<MethodInfo, IEnumerable<Attribute>> miLookup = new ConcurrentDictionary<MethodInfo, IEnumerable<Attribute>>();
        static ConcurrentDictionary<FieldInfo, IEnumerable<Attribute>> fiLookup = new ConcurrentDictionary<FieldInfo, IEnumerable<Attribute>>();
        static ConcurrentDictionary<MemberInfo, IEnumerable<Attribute>> memberiLookup = new ConcurrentDictionary<MemberInfo, IEnumerable<Attribute>>();
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
        public static IEnumerable<Attribute> Attributes(this MemberInfo methodInfo)
        {
            return memberiLookup.GetOrAdd(methodInfo, m => m.GetCustomAttributes(true).Cast<Attribute>());
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
