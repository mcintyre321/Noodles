using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FormFactory;

namespace WebNoodle.Reflection
{
    public delegate Func<object> ResolveResult(INode root, ParameterInfo parameter, object stored);
    [DebuggerDisplay("{ToString()} - Name={Name}")]
    public class ObjectMethod : IObjectMethod
    {
        private readonly MethodInfo _methodInfo;
        private readonly object _target;

        public ObjectMethod(object target, MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
            _target = target;
        }

        private BindingFlags looseBindingFlags = BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy |
                                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public string Name { get { return _methodInfo.Name; } }
        public string DisplayName { get { return Name.Replace("_", "").Sentencise(); } }
        public IEnumerable<ObjectMethodParameter> Parameters
        {
            get
            {
                var parameters = _methodInfo.GetParameters().Select(p => new ObjectMethodParameter(this, _target, _methodInfo, p)).ToArray();
                var methodName = this._methodInfo.Name.StartsWith("set_") ? _methodInfo.Name.Substring(4) : _methodInfo.Name;
                var valuesMethod = _target.GetType().GetMethod(methodName + "_values", looseBindingFlags);
                if (valuesMethod != null)
                {
                    var parameterValues = ((IEnumerable<object>) valuesMethod.Invoke(_target, new object[] {})).ToArray();
                    for (int i = 0; i < parameterValues.Length; i++)
                    {
                        parameters[i].Value = parameterValues[i];
                    }
                }
                return parameters;
            }
        }

        public void Invoke(object[] parameters)
        {
            var methodParameterInfos = _methodInfo.GetParameters();
            var resolvedParameters = new object[methodParameterInfos.Length];
            for (int index = 0; index < methodParameterInfos.Length; index++)
            {
                var parameterInfo = methodParameterInfos[index];
                resolvedParameters[index] = GetParameterValue(parameters, parameterInfo, index);
            }
            _methodInfo.Invoke(_target, resolvedParameters);
        }

        private object GetParameterValue(object[] parameters, ParameterInfo parameterInfo, int index)
        {
            if(index >= parameters.Length && parameterInfo.IsOptional) //looks there a new optional parameter has been added to the method
            {
                return null;
            }

            var value = parameters[index];

            if (value == null) //the saved parameter was null
            {
                return null;
            }
            if (value.GetType() == parameterInfo.ParameterType)
            {
                return value; 
            }

            {
                var implicitConverter = ImplicitConversionMethodHelper.ImplicitConversionMethod(value.GetType(), parameterInfo.ParameterType);
                if (implicitConverter != null)
                {
                    return implicitConverter.Invoke(null, new object[] { value });
                }
            }

            if (value is DateTime && parameterInfo.ParameterType == typeof(DateTimeOffset))
            {
                
            }

            //if (value.GetType() == typeof (JObject)) //maybe its a chunk of json
            //{
            //    var jsonObject = (JObject) value; //so deserialize as the expected type
            //    var settings = new CustomSerializerSettings();
            //    return JsonConvert.DeserializeObject(jsonObject.ToString(), parameterInfo.ParameterType, settings);
            //}
            var underlyingType = (Nullable.GetUnderlyingType(parameterInfo.ParameterType) ?? parameterInfo.ParameterType);
            if (underlyingType.IsEnum) //its an enum or a nullable enum
            {
                if (Nullable.GetUnderlyingType(parameterInfo.ParameterType) != null && string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return null; //is nullable and is empty, so must be null
                }

                if (value as long? != null) //is it an int representation of an enum?
                {
                    return Enum.ToObject(underlyingType, (long) value);
                }
                return Enum.Parse(underlyingType, value.ToString());
            }
            return To(value, parameterInfo.ParameterType); //ok so they are different types... lets try a changetype
        }
        static object To(object obj, Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
            {
                if (obj == null)
                    return null;

                return Convert.ChangeType(obj, u);
            }
            else
            {
                return Convert.ChangeType(obj, t);
            }
        }
    }
}