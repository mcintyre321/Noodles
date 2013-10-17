using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public interface IRenderHtml
    {
        IHtmlString Render(HtmlHelper html, INode node, Func<dynamic, HelperResult> renderNodeLink);
    }
}