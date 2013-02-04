using System.Reflection;

namespace Noodles.WebApi.Models
{
    public class PropertyVm
    {
        public object Value { get; set; }
        public string[] ErrorMessages { get; set; } 
        public string Type { get; set; }
        public string Name { get; set; }
        public PropertyVm(NodeProperty propertyInfo)
        {
            Name = propertyInfo.Name;
            Value = propertyInfo.Value;
            Type = propertyInfo.PropertyType.Name;
        }

        public PropertyVm(NodeMethodParameter parameter)
        {
            Name = parameter.Name;
            Value = parameter.Value;
            Type = parameter.ParameterType.Name;
        }
    }
}