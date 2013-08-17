using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CsQuery;

namespace Noodles.AspMvc.Infrastructure
{
    public class ModifyDocumentFilterAttribute : ActionFilterAttribute
    {
        private HtmlTextWriter tw;
        private StringWriter sw;
        private StringBuilder sb;
        private HttpWriter output;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            tw = new HtmlTextWriter(sw);
            output = (HttpWriter)filterContext.RequestContext.HttpContext.Response.Output;
            filterContext.RequestContext.HttpContext.Response.Output = tw;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string response = sb.ToString();
            //response processing

            var doc = new CsQuery.CQ(response);
            foreach (var tranform in filterContext.HttpContext.Items.DocTransforms())
            {
                tranform(doc);
            }
            output.Write(doc.Render());
        }
    }

    public static class DocumentResultMods
    {
        public static void AddTransform(this IDictionary items, Action<CQ> transform)
        {
            var transforms = DocTransforms(items);
            transforms.Add(transform);
        }

        public static List<Action<CQ>> DocTransforms(this IDictionary items)
        {
            if (!items.Contains("DocTransforms"))
            {
                items["DocTransforms"] = new List<Action<CQ>>();
            }
            return (List<Action<CQ>>) items["DocTransforms"];
        }
    }
}