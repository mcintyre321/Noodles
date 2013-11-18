using System;
using System.Web;
using System.Web.Mvc;
using CsQuery;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class AjaxFormAttribute : Attribute, ITransformHtml
    {
        public IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html)
        {
            var cq = CQ.Create(html);
            cq.Find("form").AddClass("ajax-form");
            return new HtmlString(cq.Render());
        }
    }
}