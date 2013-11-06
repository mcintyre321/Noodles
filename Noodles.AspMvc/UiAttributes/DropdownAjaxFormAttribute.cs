using System;
using System.Web;
using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    [NotInFormHelper]
    public class DropdownAjaxFormAttribute : Attribute, ITransformHtml
    {
        public IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html)
        {
            var form = new FormAttribute()
            {
                Ajax = true
            }.Transform(htmlHelper, node, html);
            return new DropdownAttribute().Transform(htmlHelper, node, form.ToHtmlString());
        }
    }
}