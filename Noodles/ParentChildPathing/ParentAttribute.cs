using System;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class ParentAttribute : Attribute
    {
        public static ResolveParent ResolveParentFromAttributedPropertyRule = o =>
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
        public static ResolveParent ResolveParentFromAttributedFieldRule = o =>
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
    }
}