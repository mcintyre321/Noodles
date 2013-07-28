using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Noodles.Helpers;

namespace Noodles
{
    public static class NameExtension
    {
        static ConcurrentDictionary<Type, Func<object, string>> lookup = new ConcurrentDictionary<Type, Func<object, string>>();
        //public static T SetName<T>(this T obj, string name)
        //{
        //    lookup.GetOrAdd(obj, o => name);
        //    return obj;
        //}

        public static string GetDisplayName(this object obj)
        {
            var format = lookup.GetOrAdd(obj.GetType(), (t) =>
            {
                var f = DisplayNameAttribute.GetNameAttribute(t);
                if (f == null)
                {
                    var nameFromType = t.Name.Sentencise(true);
                    return new Func<object, string>(x => nameFromType);
                }
                if (f.Item2 == null)
                {
                    var formatFromAttribute = f.Item1.Name;
                    return new Func<object, string>(x => x.ToString(formatFromAttribute));
                }
                var propertyInfo = f.Item2;
                return new Func<object, string>(x => (string) propertyInfo.GetValue(x)) ;
            });
            return format(obj);
        }
    }

    public class DisplayNameAttribute : Attribute
    {
        public string Name { get; set; }
        public DisplayNameAttribute(string name)
        {
            Name = name;
        }
        public DisplayNameAttribute()
        {
        }

        public static Tuple<DisplayNameAttribute, PropertyInfo> GetNameAttribute(Type t)
        {
            var name = t.Attributes().OfType<DisplayNameAttribute>().SingleOrDefault();
            if (name != null) return Tuple.Create(name, null as PropertyInfo);
            return t.GetProperties().Select(p => Tuple.Create(p.Attributes().OfType<DisplayNameAttribute>().SingleOrDefault(), p))
                .SingleOrDefault(tuple => tuple.Item1 != null);
        }
    }
}
