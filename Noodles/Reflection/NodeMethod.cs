using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
namespace Noodles
{
    [DebuggerDisplay("{ToString()} - Name={Name}")]
    public class NodeMethod : INodeMethod, IHasChildren, IHasParent<NodeMethods>
    {
        private readonly NodeMethods _parent;
        private readonly MethodInfo _methodInfo;

        public NodeMethod(object behaviour, NodeMethods parent, MethodInfo methodInfo)
        {
            _parent = parent;
            _methodInfo = methodInfo;
            Target = behaviour;
        }

        public object Target { get; private set; }

        public string SuccessMessage
        {
            get { return null; } //TODO: need a system for specify friendly success messages
        }

        private BindingFlags looseBindingFlags = BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        [Name]
        public string Name { get { return _methodInfo.Name; } }

        public string DisplayName
        {
            get
            {
                return _displayName ?? (_displayName = GetDisplayName());
            }
        }

        private string GetDisplayName()
        {
            var att = this._methodInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }

        private string _displayName;


        private IEnumerable<NodeMethodParameter> _parameters;

        public IEnumerable<NodeMethodParameter> Parameters
        {
            get
            {
                return _parameters ?? (_parameters = LoadParameters());
            }
        }

        private IEnumerable<NodeMethodParameter> LoadParameters()
        {
            var parameters = _methodInfo.GetParameters().Select(p => new NodeMethodParameter(this, Target, _methodInfo, p)).ToArray();
            var methodName = this._methodInfo.Name.StartsWith("set_")
                                 ? _methodInfo.Name.Substring(4)
                                 : _methodInfo.Name;
            var valuesMethod = Target.GetType().GetMethod(methodName + "_values", looseBindingFlags);
            if (valuesMethod != null)
            {
                var parameterValues = ((IEnumerable<object>)valuesMethod.Invoke(Target, new object[] { })).ToArray();
                for (int i = 0; i < parameterValues.Length; i++)
                {
                    parameters[i].Value = parameterValues[i];
                }
            }
            return parameters;
        }

        public void Invoke(object[] parameters)
        {
            var methodParameterInfos = this.Parameters.ToArray();
            for (int index = 0; index < methodParameterInfos.Length; index++)
            {
                var methodParameter = methodParameterInfos[index];
                var resolvedParameterValue = GetParameterValue(parameters, methodParameter.ParameterInfo, index);
                methodParameterInfos[index].LastValue = resolvedParameterValue;
            }
            _methodInfo.Invoke(Target, methodParameterInfos.Select(mp => mp.LastValue).ToArray());
        }

        private object GetParameterValue(object[] parameters, ParameterInfo parameterInfo, int index)
        {
            if (index >= parameters.Length && parameterInfo.IsOptional) //looks there a new optional parameter has been added to the method
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
                    return Enum.ToObject(underlyingType, (long)value);
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

        
        public object GetChild(string name)
        {
            return this.Parameters.SingleOrDefault(p => p.Name == name);
        }

        public NodeMethods Parent
        {
            get { return _parent; }
        }
    }
}