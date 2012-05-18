using System.Web.Mvc;
using System.Web.Mvc;
using FormFactory;

namespace Noodles.Helpers
{
    public static class FormFactoryHelperExtensions
    {
        public static PropertyVm ToPropertyVm(this NodeMethodParameter parameter, HtmlHelper Html)
        {
            return new PropertyVm(Html, parameter.ParameterType, parameter.Name)
            {
                Id = () => parameter.Id(), 
                DisplayName = parameter.DisplayName,
                GetCustomAttributes = () => parameter.CustomAttributes,
                Readonly = false,
                Value = parameter.Value,
                Choices = parameter.Choices,
                Suggestions = parameter.Suggestions,
                Source = parameter,

            };
        }
    }
}
