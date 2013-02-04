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
        static ConcurrentDictionary<Type, string> lookup = new ConcurrentDictionary<Type, string>();
        public static string GetName(this object o)
        {
            var type = o.GetType();
            var format = lookup.GetOrAdd(type, (t) =>
            {
                var name = t.GetCustomAttributes(false).OfType<NameAttribute>().SingleOrDefault();
                if (name == null) return o.GetType().Name.Sentencise();
                return name.Name;
            });
            return FormattableObject.ToString(o, format);
            ;
        }
    }

    public class NameAttribute : Attribute
    {
        public string Name { get; set; }
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
