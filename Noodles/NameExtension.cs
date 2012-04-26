using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
                GetNameFromAttribute,
                GetNameFromMetaProps
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
            var attributedProperty = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty)
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

        private static readonly ConditionalWeakTable<object, string> Names = new ConditionalWeakTable<object, string>();

        public static Noodles.ResolveName GetNameFromMetaProps = o => Names.GetValue(o, (c) => null);

        public static string Name(this object obj)
        {
            return NameRules.Select(r => r(obj)).FirstOrDefault(name => name != null);
        }

        public static void SetName(this object o, string name)
        {
            if (name == null)
            {
                Names.Remove(o);
            }
            else
            {
                Names.Remove(o);
                Names.Add(o, name);
            }
        }


    }

    public class NameAttribute : Attribute
    {
    }
}