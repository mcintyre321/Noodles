using System.Reflection;

namespace Noodles.Example.WebApi.Models
{
    public class PropertyVm
    {
        public object Value { get; set; }
        public string[] ErrorMessages { get; set; } 
        public string Type { get; set; }
        public string Name { get; set; }
        public bool ReadOnly { get; set; }
        public ResourceVm Setter { get; set; } 
        public PropertyVm(object value, PropertyInfo propertyInfo)
        {
            Name = propertyInfo.Name;
            Value = propertyInfo.GetValue(value, null);
            Type = propertyInfo.PropertyType.Name;
            var method = value.NodeMethod("set_" + Name);
            if (method != null)
            {
                Setter = new ResourceVm(method);
            }
            ReadOnly = Setter == null;
        }

        public PropertyVm(NodeMethodParameter parameter)
        {
            Name = parameter.Name;
            Value = parameter.LastValue;
            Type = parameter.ParameterType.Name;
            ReadOnly = false;
        }
    }
}