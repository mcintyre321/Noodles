using System;
using Noodles.Helpers;

namespace Noodles.Attributes
{
    public class DefaultAttribute : Attribute
    {
        public object Value { get; set; }

        public DefaultAttribute(object value)
        {
            Value = value;
        }

        public object GetValue(object target)
        {
            if (Value is string)
            {
                return target.ToString((string) Value);
            }
            return Value;
        }
    }
}