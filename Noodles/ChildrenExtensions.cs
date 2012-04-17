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

    public delegate object ResolveChild(object node, string childName);

    public delegate bool? AllowGetChildFromPropertyInfoRule(object node, PropertyInfo pi);
    //public class ChildAttribute : Attribute
    //{
    //    private readonly string _name;
    //    private static List<AllowGetChildFromPropertyInfoRule> Rules;

    //    static ChildAttribute()
    //    {
    //        Rules = new List<AllowGetChildFromPropertyInfoRule>();
    //    }

    //    public ChildAttribute(){}
    //    public ChildAttribute(string name): this()
    //    {
    //        _name = name;
    //    }

    //    public object GetChild(object obj, PropertyInfo pi)
    //    {
    //        var allow = Rules.Select(r => r(obj, pi)).FirstOrDefault(r => r != null) ?? true;
    //        if (allow)
    //        {
    //            var child = pi.GetValue(obj, null);
    //            if (child.Name() == null)
    //            {
    //                child.SetName(_name ?? pi.Name);
    //            }
    //            return child;
    //        }
    //        return null;
    //    }
    //}

    public static class ChildrenExtensions
    {
        static ChildrenExtensions()
        {
            Rules = new List<ResolveChild>()
            {
                ResolveChildFromIHasChildren,
                //ResolveChildFromAttributedProperty,
                //ResolveChildFromAttributedField,
                ResolveChildFromIEnumerable,
                ResolveChildFromMetaProps_Children,

            };
        }

        public static List<ResolveChild> Rules;
        public static object GetChild(this object o, string name)
        {
            return Rules.Select(r => r(o, name)).FirstOrDefault(child => child != null);
        }

        public static ResolveChild ResolveChildFromMetaProps_Children = (o, name) =>
        {
            var children = o.Meta()["Children"] as Hashtable;
            if (children != null)
            {
                return children[name];
            }
            return null;
        };
        
        public static ResolveChild ResolveChildFromIHasChildren = (object o, string name) =>
        {
            if (o is IHasChildren)
            {
                return ((IHasChildren) o).GetChild(name);
            }
            return null;
        };
        //public static ResolveChild ResolveChildFromAttributedProperty = (object o, string name) =>
        //{
        //    var attributedProperties = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty)
        //          .Select(x => new
        //          {

        //              Member = x,
        //              Attribute = x.GetCustomAttributes(typeof(ChildAttribute), true).FirstOrDefault() as ChildAttribute
        //          })
        //          .SingleOrDefault(x => (x.Member.GetValue(o, null) != null && x.Member.GetValue(o, null).Name() == name));
        //    if (attributedProperties != null)
        //    {
        //        var value = attributedProperties.Member.GetValue(o, null);
        //        if (value == null)
        //        {
        //            throw new Exception("Properties with ChildAttribute cannot return null - " + attributedProperties.Member.DeclaringType.FullName + "." + attributedProperties.Member.Name + " did");
        //        }
        //        return value;
        //    }
        //    return null;
        //};

        //public static ResolveChild ResolveChildFromAttributedField = (object o, string name) =>
        //{
        //    var attributedFields = o.GetType().GetFields(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField)
        //          .Select(x => new
        //          {
        //              Member = x,
        //              Attribute = x.GetCustomAttributes(typeof(ChildAttribute), true).FirstOrDefault() as ChildAttribute
        //          })
        //          .SingleOrDefault(x => x.Attribute != null && x.Member.GetValue(o).Name() == name);
        //    if (attributedFields != null)
        //    {
        //        var value = attributedFields.Member.GetValue(o);
        //        if (value == null)
        //        {
        //            throw new Exception("Fields with ChildAttribute cannot return null - " + attributedFields.Member.DeclaringType.FullName + "." + attributedFields.Member.Name + " did");
        //        }
        //        return value;
        //    }
        //    return null;
        //};

        public static ResolveChild ResolveChildFromIEnumerable = (o, name) =>
        {
            if (o is IEnumerable)
            {
                return ((IEnumerable) o).Cast<object>().FirstOrDefault(x => x.Name() == name);
            }
            return null;
        };

    }
}