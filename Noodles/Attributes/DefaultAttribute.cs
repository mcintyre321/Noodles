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
                var stringValue = Value as string;
                if (stringValue.StartsWith("{") && stringValue.EndsWith("}"))
                {
                    return target.GetType()
                        .GetProperty(stringValue.Substring(1, stringValue.Length - 2))
                        .GetValue(target);
                }
                return target.ToString((string) Value);
            }
            return Value;
        }
    }
}