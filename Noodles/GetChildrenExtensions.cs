using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public interface IHasChildren
    {
        object GetChild(string name);
    }

    public delegate object GetChildRule(object node, string childName);

    public delegate bool? AllowGetChildFromPropertyInfoRule(object node, PropertyInfo pi);
    public class ChildAttribute : Attribute
    {
        private static List<AllowGetChildFromPropertyInfoRule> Rules;

        static ChildAttribute()
        {
            Rules = new List<AllowGetChildFromPropertyInfoRule>();
        }


        public object GetChild(object obj, PropertyInfo pi)
        {
            var allow = Rules.Select(r => r(obj, pi)).FirstOrDefault(r => r != null) ?? true;
            return allow ? pi.GetValue(obj, null) : null;
            return null;
        }
    }

    public static class GetChildrenExtensions
    {
        static GetChildrenExtensions()
        {
            Rules = new List<GetChildRule>()
            {
                GetChildFromIHasChildren,
                GetChildFromAttributedProperty
            };
        }

        public static List<GetChildRule> Rules;
        public static object GetChild(this object o, string name)
        {
            return Rules.Select(r => r(o, name)).FirstOrDefault(child => child != null);
        }

        
        public static GetChildRule GetChildFromIHasChildren = (object o, string name) =>
        {
            if (o is IHasChildren)
            {
                return ((IHasChildren) o).GetChild(name);
            }
            return null;
        };
        public static GetChildRule GetChildFromAttributedProperty = (object o, string name) =>
        {
            var propertyInfo = o.GetType().GetProperty(name);
            if (propertyInfo != null)
            {
                var attribute = propertyInfo.GetCustomAttributes(typeof (ChildAttribute), true)
                                    .FirstOrDefault() as ChildAttribute;

                if (attribute != null) return attribute.GetChild(o, propertyInfo);
            }
            return null;
        };
    }
}