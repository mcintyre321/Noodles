using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using FormFactory;

namespace Noodles.AspMvc.Helpers
{
    public static class FormFactoryHelperExtensions
    {
        public static PropertyVm ToPropertyVm(this NodeMethodParameter parameter, HtmlHelper Html)
        {
            var vm = new PropertyVm(Html, parameter.ParameterType, parameter.Name)
            {
                DisplayName = parameter.DisplayName,
                GetCustomAttributes = () => parameter.CustomAttributes,
                Readonly = false,
                IsHidden = parameter.CustomAttributes.OfType<DataTypeAttribute>().Any(x => x.CustomDataType == "Hidden"),
                Value = parameter.Value,
                Choices = parameter.Choices,
                Suggestions = parameter.Suggestions,
                Source = parameter,
            };
            vm.IsHidden |= parameter.Locked;
            return vm;
        }
        public static PropertyVm ToPropertyVm(this NodeProperty property, HtmlHelper Html)
        {

            var vm = new PropertyVm(Html, property.PropertyType, property.Name)
            {
                DisplayName = property.DisplayName,
                GetCustomAttributes = () => property.CustomAttributes,
                Readonly = true,
                Value = property.Value,
                IsHidden = false, //parameter.CustomAttributes.OfType<DataTypeAttribute>().Any(x => x.CustomDataType == "Hidden"),
                Source = property,
            };
            return vm;
        }
    }
}
