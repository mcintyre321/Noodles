using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public interface ITransformHtml
    {
        IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html);
    }
}