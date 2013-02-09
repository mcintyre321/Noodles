using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class NodeProperty
    {
        private readonly Func<IEnumerable<Attribute>> _getCustomAttributes; 
        public NodeProperty(object target, PropertyInfo info)
        {
            _getCustomAttributes = info.GetCustomAttributes;
            Value = info.GetValue(target, null);
            PropertyType = info.PropertyType;
            Name = info.Name;
            DisplayName = GetDisplayName(info);
            Readonly = target.NodeMethod("set_" + this.Name) == null;
        }
        public NodeProperty(object target, FieldInfo info)
        {
            _getCustomAttributes = info.GetCustomAttributes;
            Value = info.GetValue(target);
            PropertyType = info.FieldType;
            Name = info.Name;
            DisplayName = GetDisplayName(info);
            Readonly = target.NodeMethod("set_" + this.Name) == null;
        }


        public string DisplayName { get; private set; }
        public Type PropertyType { get; private set; }
        public object Value { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<object> CustomAttributes
        {
            get { return _getCustomAttributes(); }
        }

        public bool Readonly { get; private set; }

        string GetDisplayName(PropertyInfo info)
        {
            var att = info.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }
        string GetDisplayName(FieldInfo info)
        {
            var att = info.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }

    }
}