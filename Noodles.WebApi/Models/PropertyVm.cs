using System.Reflection;
using Noodles.Models;

namespace Noodles.WebApi.Models
{
    public class PropertyVm
    {
        public object Value { get; set; }
        public string[] ErrorMessages { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool ReadOnly { get; set; }
        //public PropertyVm(NodeProperty property)
        //{
        //    Name = property.Name;
        //    Type = property.PropertyType.Name;
          
        //}

        public PropertyVm(NodeMethodParameter parameter)
        {
            Name = parameter.Name;
            Value = parameter.LastValue;
            Type = parameter.ParameterType.Name;
            ReadOnly = false;
        }
    }
}