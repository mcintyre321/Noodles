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
    public class RenderFfAttribute : Attribute, IRenderHtml
    {
        public IHtmlString Render(HtmlHelper html, INode node, Func<dynamic, HelperResult> renderNodeLink)
        {
            var propertyVm = node.ToPropertyVm();
            return html.Partial(html.BestViewName(propertyVm.Type, "FormFactory/Property."), propertyVm);
        }
    }
}