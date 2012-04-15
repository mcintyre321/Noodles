using System;
using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public delegate string ResolveName(object child);

    public interface IHasName
    {
        string Name { get; }
    }

    public static class NameExtension
    {
        static NameExtension()
        {
            NameRules = new List<Noodles.ResolveName>()
            {
                GetNameFromInterface,
                GetNameFromAttribute
            };
        }

        public static Noodles.ResolveName GetNameFromInterface = o =>
        {
            var hasName = o as IHasName;
            if (hasName != null)
            {
                return (hasName).Name;
            }
            return null;
        };
        public static Noodles.ResolveName GetNameFromAttribute = o =>
        {
            var attributedProperty = o.GetType().GetProperties()
                .Select(pi => new
                        {
                            Property = pi,
                            Attribute = pi.GetCustomAttributes(typeof (NameAttribute), true).FirstOrDefault()
                        })
                .SingleOrDefault(p => p.Attribute != null);
            if (attributedProperty != null)
            {
                var value = attributedProperty.Property.GetValue(o, null);
                if (value == null)
                {
                    throw new Exception("Properties with NameAttribute cannot return null - " + attributedProperty.Property.DeclaringType.FullName + "." + attributedProperty.Property.Name + " did");
                }
                return value.ToString();
            }
            return null;
        };


        public static List<Noodles.ResolveName> NameRules;


        public static string Name(this object obj)
        {
            return NameRules.Select(r => r(obj)).FirstOrDefault(name => name != null);
        }
    }

    public class NameAttribute : Attribute
    {
    }
}