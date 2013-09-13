using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using FormFactory;
using Noodles.AspMvc.Helpers;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class RenderViewAttribute : Attribute, IRenderHtml
    {
        public string ViewName { get; set; }
        public IHtmlString Render(HtmlHelper html, INode node)
        {
            return html.Partial(ViewName, node);
        }
    }
    public class RenderFfAttribute : Attribute, IRenderHtml
    {
        public IHtmlString Render(HtmlHelper html, INode node)
        {
            var propertyVm = node.ToPropertyVm();
            return html.Partial(html.BestViewName(propertyVm.Type, "FormFactory/Property."), propertyVm);
        }
    }

    public interface IRenderHtml
    {
        IHtmlString Render(HtmlHelper html, INode node);
    }
}