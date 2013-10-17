using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class RenderViewAttribute : Attribute, IRenderHtml
    {
        public string ViewName { get; set; }
        public IHtmlString Render(HtmlHelper html, INode node, Func<dynamic, HelperResult> renderNodeLink)
        {
            return html.Partial(ViewName, node);
        }
    }
}