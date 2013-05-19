using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class ClickToEditAttribute : Attribute
    {
    }

    public static class NodeMethodGetFormGroupsExtension
    {
        public static IEnumerable<FormInfo> GetFormGroups(this IInvokeable invokeable)
        {
            var singleFormParameters =
                invokeable.Parameters.Where(x => !x.CustomAttributes.OfType<ClickToEditAttribute>().Any());
            if (singleFormParameters.Any())
                yield return new FormInfo(invokeable, singleFormParameters);
        }
        public static IEnumerable<IInvokeableParameter> GetSingleSettableProperies(this IInvokeable invokeable)
        {
            return invokeable.Parameters.Where(x => x.CustomAttributes.OfType<ClickToEditAttribute>().Any());
        }

    }
}