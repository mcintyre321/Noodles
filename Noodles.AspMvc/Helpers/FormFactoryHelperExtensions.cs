using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using FormFactory;
using Noodles.Models;

namespace Noodles.AspMvc.Helpers
{
    public static class FormFactoryHelperExtensions
    {
        static object GetAttemptedValue(this HtmlHelper helper, string modelKey)
        {
            ModelState modelState;
            if (helper.ViewData.ModelState.TryGetValue(modelKey, out modelState))
            {
                return modelState.Value.AttemptedValue;
            }
            return null;
        }

        public static PropertyVm ToPropertyVm(this IInvokeableParameter parameter, HtmlHelper html)
        {
            var vm = new PropertyVm(html, parameter.ValueType, parameter.Name)
            {
                DisplayName = parameter.DisplayName,
                GetCustomAttributes = () => parameter.CustomAttributes,
                Readonly = false,
                IsHidden = parameter.CustomAttributes.OfType<DataTypeAttribute>().Any(x => x.CustomDataType == "Hidden"),
                Value = parameter.LastValue ?? parameter.Value,
                Choices = parameter.Choices,
                Suggestions = parameter.Suggestions,
                Source = parameter,
            };
            vm.IsHidden |= parameter.Locked;
            return vm;
        }
         

        public static PropertyVm ToPropertyVm(this NodeProperty property, HtmlHelper html)
        {
            var vm = new PropertyVm(html, property.ValueType, property.Name)
            {
                DisplayName = property.DisplayName,
                GetCustomAttributes = () => property.CustomAttributes,
                Readonly = property.Readonly,
                Value = property.Value,
                Choices = ((IInvokeableParameter)property).Choices,
                Suggestions = ((IInvokeableParameter)property).Suggestions,
                IsHidden = false, //parameter.CustomAttributes.OfType<DataTypeAttribute>().Any(x => x.CustomDataType == "Hidden"),
                Source = property,
            };
            return vm;
        }
    }
}
