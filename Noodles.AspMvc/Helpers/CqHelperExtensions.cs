using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using CsQuery;

namespace Noodles.AspMvc.Helpers
{
    public static class CqHelperExtensions
    {
        public static IHtmlString Transform(this HelperResult hr, Action<CQ> transform)
        {
            var htmlString = hr.ToHtmlString();
            var html = Transform(htmlString, transform);
            return html;
        }

        private static HtmlString Transform(this string htmlString, Action<CQ> transform)
        {
            var cq = CsQuery.CQ.Create(htmlString);
            transform(cq);
            var html = new HtmlString(cq.Render());
            return html;
        }

        public static CQ Attr(this CQ cq, string attrName, Func<string, string> transformAttribute)
        {
            cq.Attr(attrName, transformAttribute(cq.Attr(attrName)));
            return cq;
        }

    }
}