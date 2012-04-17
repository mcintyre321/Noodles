using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public interface IHasParent<out T>
    {
        T Parent { get; }
    }
    public delegate object ResolveParent(object child);

    public class ParentAttribute : Attribute
    {

    }
    public static class ParentExtensions
    {
        public static object Parent(this object child)
        {
            return ParentRules.Select(r => r(child)).FirstOrDefault(parent => parent != null);
        }
        public static ResolveParent GetParentFromIHasParent = o =>
        {
            var hasParent = o as IHasParent<object>;
            if (hasParent != null)
            {
                return (hasParent).Parent;
            }
            return null;
        };

        public static ResolveParent GetParentFromAttributedProperty = o =>
        {
            var attributedProperty = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty)
                .Select(pi => new
                        {
                            Property = pi,
                            Attribute = pi.GetCustomAttributes(typeof(ParentAttribute), true).FirstOrDefault()
                        })
                .SingleOrDefault(p => p.Attribute != null);
            if (attributedProperty != null)
            {
                var value = attributedProperty.Property.GetValue(o, null);
                if (value == null)
                {
                    throw new Exception("Properties with ParentAttribute cannot return null - " + attributedProperty.Property.DeclaringType.FullName + "." + attributedProperty.Property.Name + " did");
                }
                return value;
            }
            return null;
        };

        public static ResolveParent GetParentFromAttributedField = o =>
        {
            var attributedProperty = o.GetType().GetFields(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField)
                .Select(fi => new
                {
                    Field = fi,
                    Attribute = fi.GetCustomAttributes(typeof(ParentAttribute), true).FirstOrDefault()
                })
                .SingleOrDefault(p => p.Attribute != null);
            if (attributedProperty != null)
            {
                var value = attributedProperty.Field.GetValue(o);
                if (value == null)
                {
                    throw new Exception("Fields with ParentAttribute cannot return null - " + attributedProperty.Field.DeclaringType.FullName + "." + attributedProperty.Field.Name + " did");
                }
                return value;
            }
            return null;
        };
        static ParentExtensions()
        {
            ParentRules = new List<ResolveParent>()
            {
                GetParentFromIHasParent,
                GetParentFromAttributedProperty,
                GetParentFromAttributedField,
                GetParentFromMetaProps
            };
        }

        public static ResolveParent GetParentFromMetaProps = o => o.Meta()["Parent"];

        public static List<ResolveParent> ParentRules;


        public static T SetParent<T>(this T child, object parent, string name = "_")
        {
            child.Meta()["Parent"] = parent;
            var children = parent.Meta()["Children"] as Hashtable;
            if (children == null)
            {
                children = new Hashtable();
                parent.Meta()["Children"] = children;
            }
            var safeName = name;
            if (child.Name() == null)
            {
                var nameCounter = 0;
                while (children.GetChild(safeName) != null)
                {
                    nameCounter++;
                }
                safeName = name + (nameCounter == 0 ? "" : nameCounter.ToString());
                child.SetName(safeName);
            }
            children[child.Name()] = child;
            return child;
        }
    }
}