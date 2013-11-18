using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Noodles.AspMvc.Helpers;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    /// <summary>
    /// Causes a nodelink to be rendered as a dropdown containing it's update form
    /// </summary>
    [NotInFormHelper]
    public class DropdownAttribute : Attribute, ITransformHtml
    {
        public IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html)
        {
            var formHtml = html;
            var liContainingForm = new MvcHtmlString("<li>" + formHtml  + "</li>");
            var dropdownLinksButton = BootstrapHelper.DropdownLinksButton(node.DisplayName, liContainingForm);
            return dropdownLinksButton;
        }

    }
}