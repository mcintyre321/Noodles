using System;
using System.Collections;
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

    public static class ChildrenExtensions
    {
        static ChildrenExtensions()
        {
            Rules = new List<GetChildRule>()
            {
                GetChildFromIHasChildren,
                GetChildFromAttributedProperty,
                GetChildFromAttributedField,
                GetChildFromIEnumerable
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
            var attributedProperties = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty)
                  .Select(x => new
                  {
                      Member = x,
                      Attribute = x.GetCustomAttributes(typeof(ChildAttribute), true).FirstOrDefault() as ChildAttribute
                  })
                  .SingleOrDefault(x => x.Attribute != null && (x.Member.GetValue(o, null).Name() == name));
            if (attributedProperties != null)
            {
                var value = attributedProperties.Member.GetValue(o, null);
                if (value == null)
                {
                    throw new Exception("Properties with ChildAttribute cannot return null - " + attributedProperties.Member.DeclaringType.FullName + "." + attributedProperties.Member.Name + " did");
                }
                return value;
            }
            return null;
        };

        public static GetChildRule GetChildFromAttributedField = (object o, string name) =>
        {
            var attributedFields = o.GetType().GetFields(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField)
                  .Select(x => new
                  {
                      Member = x,
                      Attribute = x.GetCustomAttributes(typeof(ChildAttribute), true).FirstOrDefault() as ChildAttribute
                  })
                  .SingleOrDefault(x => x.Attribute != null && x.Member.GetValue(o).Name() == name);
            if (attributedFields != null)
            {
                var value = attributedFields.Member.GetValue(o);
                if (value == null)
                {
                    throw new Exception("Fields with ChildAttribute cannot return null - " + attributedFields.Member.DeclaringType.FullName + "." + attributedFields.Member.Name + " did");
                }
                return value;
            }
            return null;
        };

        public static GetChildRule GetChildFromIEnumerable = (o, name) =>
        {
            if (o is IEnumerable)
            {
                return ((IEnumerable) o).Cast<object>().FirstOrDefault(x => x.Name() == name);
            }
            return null;
        };
    }
}