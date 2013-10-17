using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using CsQuery;
using Noodles.AspMvc.Helpers;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    /// <summary>
    /// Causes a nodelink to be rendered as a dropdown containing it's update form
    /// </summary>
    public class DropdownAttribute : Attribute, IRenderHtml
    {
        public IHtmlString Render(HtmlHelper html, INode node, Func<dynamic, HelperResult> renderNodeLink)
        {
            var formHtml = NoodlesHelper.Form(html, node);
            var liContainingForm = new MvcHtmlString("<li>" + formHtml.ToHtmlString() + "</li>");
            var dropdownLinksButton = BootstrapHelper.DropdownLinksButton(node.DisplayName, liContainingForm);
            return dropdownLinksButton;
        }
    }
    public class AutoSubmitAttribute : Attribute, IRenderHtml
    {
        public IHtmlString Render(HtmlHelper html, INode node, Func<dynamic, HelperResult> renderNodeLink)
        {
            var cq = CQ.Create(renderNodeLink(null).ToHtmlString());
            cq.AddClass("auto-submit");
            return new HtmlString(cq.Render());
        }
    }
}