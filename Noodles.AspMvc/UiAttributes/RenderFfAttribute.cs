using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using FormFactory;
using Noodles.AspMvc.Helpers;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class TransformFfAttribute : Attribute, ITransformHtml
    {
        public IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html)
        {
            var propertyVm = node.ToPropertyVm();
            var partialViewName = htmlHelper.BestViewName(propertyVm.Type, "FormFactory/Property.");
            return htmlHelper.Partial(partialViewName, propertyVm);
        }
    }
}