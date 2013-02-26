using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noodles.Helpers;

namespace Noodles
{
    public static class NameExtension
    {
        static ConcurrentDictionary<object, string> lookup = new ConcurrentDictionary<object, string>();
        public static T SetName<T>(this T obj, string name)
        {
            lookup.GetOrAdd(obj, o => name);
            return obj;
        }

        public static string GetDisplayName(this object obj)
        {
            var format = lookup.GetOrAdd(obj, (o) =>
            {
                var f = DisplayNameAttribute.GetNameAttribute(o.GetType());
                if (f == null) f = o.GetType().Name.Sentencise(true);
                return f;

            });
            return obj.ToString(format);
            ;
        }
    }

    public class DisplayNameAttribute : Attribute
    {
        static ConcurrentDictionary<Type, string> lookup = new ConcurrentDictionary<Type, string>();

        public string Name { get; set; }
        public DisplayNameAttribute(string name)
        {
            Name = name;
        }

        public static string GetNameAttribute(Type t)
        {
            var name = t.GetCustomAttributes(false).OfType<DisplayNameAttribute>().SingleOrDefault();
            if (name == null) return null;
            return name.Name;
        }
    }
}
