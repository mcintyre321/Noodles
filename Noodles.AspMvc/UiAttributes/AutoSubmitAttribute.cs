using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using CsQuery;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class AutoSubmitAttribute : Attribute, ITransformHtml
    {
        public IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html)
        {
            var cq = CQ.Create(html);
            cq.AddClass("auto-submit");
            return new HtmlString(cq.Render());
        }
    }
}