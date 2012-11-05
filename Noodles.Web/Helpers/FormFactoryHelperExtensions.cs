using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc;
using FormFactory;

namespace Noodles.Helpers
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
    }
}
