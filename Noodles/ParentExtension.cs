using System;
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
    public static class ParentExtension
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
            var attributedProperty = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.GetProperty)
                .Select(pi => new
                        {
                            Property = pi,
                            Attribute = pi.GetCustomAttributes(typeof (ParentAttribute), true).FirstOrDefault()
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
            var attributedProperty = o.GetType().GetFields(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.GetField)
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
        static ParentExtension()
        {
            ParentRules = new List<ResolveParent>()
            {
                GetParentFromIHasParent,
                GetParentFromAttributedProperty,
                GetParentFromAttributedField
            };
        }

        public static List<ResolveParent> ParentRules;

    }
}