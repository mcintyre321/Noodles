using System.Collections.Generic;
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
            System.Web.Mvc.ModelState modelState;
            if (helper.ViewData.ModelState.TryGetValue(modelKey, out modelState))
            {
                return modelState.Value.AttemptedValue;
            }
            return null;
        }

        public static PropertyVm ToPropertyVm(this IInvokeableParameter parameter, HtmlHelper html)
        {
            var customAtts = new List<object>();
            if (parameter.IsOptional == false) customAtts.Add(new RequiredAttribute());
            var vm = html.CreatePropertyVm(parameter.ValueType, parameter.Name);

            {
                vm.DisplayName = parameter.DisplayName;
                vm.GetCustomAttributes = () => parameter.CustomAttributes.Concat(customAtts);
                vm.Readonly = parameter.Readonly;
                vm.IsHidden = parameter.CustomAttributes.OfType<DataTypeAttribute>().Any(x => x.CustomDataType == "Hidden");
                vm.Value = parameter.LastValue ?? parameter.Value;
                vm.Choices = parameter.Choices;
                vm.Suggestions = parameter.Suggestions;
                vm.Source = parameter;
            };
            vm.IsHidden |= parameter.Locked;
            return vm;
        }
         

        public static PropertyVm ToPropertyVm(this NodeProperty property, HtmlHelper html)
        {
            return ((IInvokeableParameter)property).ToPropertyVm(html);
        }
    }
}
