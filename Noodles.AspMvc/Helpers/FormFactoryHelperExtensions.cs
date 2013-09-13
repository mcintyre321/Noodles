using System;
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
        public static PropertyVm ToPropertyVm(this IInvokeableParameter parameter)
        {
            var customAtts = new List<object>();
            var vm = new PropertyVm(parameter.ValueType, parameter.Name);

            {
                vm.DisplayName = parameter.DisplayName;
                vm.GetCustomAttributes = () => parameter.Attributes.Concat(customAtts);
                vm.Readonly = parameter.Readonly;
                vm.IsHidden = parameter.Attributes.OfType<DataTypeAttribute>().Any(x => x.CustomDataType == "Hidden");
                vm.Value = parameter.Value;
                vm.Choices = parameter.Choices;
                vm.Suggestions = parameter.Suggestions;
                vm.Source = parameter;
            };
            return vm;
        }
         
        public static PropertyVm ToPropertyVm(this INode node)
        {
            if (node is NodeProperty)
            {
                return ((NodeProperty)node).ToPropertyVm();
            } 
            
            throw new Exception("Cannot render as FormFactory property VM - type " + node + " is not supported.");
        }

        public static PropertyVm ToPropertyVm(this NodeProperty property)
        {
            return ((IInvokeableParameter)property).ToPropertyVm();
        }
    }
}
