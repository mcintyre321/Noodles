using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Noodles.Reflection
{
    public delegate bool ParameterValueGetter(object[] parameters, ParameterInfo parameterInfo, int index, out object result);

    public static class NodeMethodParameterFixes
    {


        public static readonly ParameterValueGetter ImplicitCast =
            (object[] parameters, ParameterInfo parameterInfo, int index, out object result) =>
            {
                var value = parameters[index];
                var implicitConverter = ImplicitConversionMethodHelper.ImplicitConversionMethod(value.GetType(),
                                                                                                parameterInfo.
                                                                                                    ParameterType);
                if (implicitConverter != null)
                {
                    result = implicitConverter.Invoke(null, new object[] { value });
                    return true;
                }
                result = null;
                return false;
            };

        public static readonly ParameterValueGetter Downcast =
            (object[] parameters, ParameterInfo parameterInfo, int index, out object result) =>
            {
                result = parameters[index];
                return parameterInfo.ParameterType.IsAssignableFrom(result.GetType());
            };

        public static readonly ParameterValueGetter NullOnNewOptional =
            (object[] parameters, ParameterInfo parameterInfo, int index, out object result) =>
            {
                result = null;
                return index >= parameters.Length && parameterInfo.IsOptional;
            };

        public static readonly ParameterValueGetter FixEnumTypes =
            (object[] parameters, ParameterInfo parameterInfo, int index, out object result) =>
            {
                var underlyingType = (Nullable.GetUnderlyingType(parameterInfo.ParameterType) ??
                                      parameterInfo.ParameterType);
                if (underlyingType.IsEnum) //its an enum or a nullable enum
                {
                    var value = parameters[index];
                    if (Nullable.GetUnderlyingType(parameterInfo.ParameterType) != null &&
                        string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        result = null;
                        return true;
                    }

                    if (value as long? != null) //is it an int representation of an enum?
                    {
                        result = Enum.ToObject(underlyingType, (long)value);
                        return true;
                    }
                    result = Enum.Parse(underlyingType, value.ToString());
                    return true;
                }
                result = null;
                return false;
            };

        public static readonly ParameterValueGetter NullToNull =
            (object[] parameters, ParameterInfo parameterinfo, int index, out object result) =>
            {
                result = null;
                var value = parameters[index];
                return value == null;
            };

        public static ParameterValueGetter ChangeType = (object[] parameters, ParameterInfo parameterInfo, int index, out object result) =>
        {
            var obj = parameters[index];
            var t = parameterInfo.ParameterType;
            Type u = Nullable.GetUnderlyingType(t);
            if (u != null)
            {
                if (obj == null)
                {
                    result = null;
                    return false;
                }
                result = Convert.ChangeType(obj, u);
                return true;
            }
            result = Convert.ChangeType(obj, u);
            return true;
        };

        public static List<ParameterValueGetter> Registry = new List<ParameterValueGetter>()
        {
            NullOnNewOptional,
            NullToNull,
            FixEnumTypes,
            Downcast,
            ImplicitCast,
            ChangeType
        };

        
    }
}
