using System;
using System.Web;
using System.Web.Mvc;
using Noodles.AspMvc.Helpers;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class DropdownAttribute : Attribute, IRenderHtml
    {
        public IHtmlString Render(HtmlHelper html, INode node)
        {
            var formHtml = NoodlesHelper.Form(html, node);
            var liContainingForm = new MvcHtmlString("<li>" + formHtml.ToHtmlString() + "</li>");
            var dropdownLinksButton = BootstrapHelper.DropdownLinksButton(node.DisplayName, liContainingForm);
            return dropdownLinksButton;
        }
    }
}