using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FormFactory;

namespace WebNoodle.Helpers
{
    static class ForFactoryHelperExtensions
    {
        public static PropertyVm ToPropertyVm(this ObjectMethodParameter parameter, HtmlHelper Html)
        {
             return new PropertyVm(Html, parameter.ParameterType, parameter.Name, parameter.Id(), parameter.DisplayName)
            {
                GetCustomAttributes = () => parameter.CustomAttributes,
                IsWritable = true,
                Value = parameter.Value,
                Choices = parameter.Choices,
                Suggestions = parameter.Suggestions,
                Source = parameter

            };
        }
    }
}
