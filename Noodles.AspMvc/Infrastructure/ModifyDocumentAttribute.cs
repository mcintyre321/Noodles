using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Items.SuppressDocTransforms()
            && filterContext.Result is ViewResult
            && filterContext.Exception == null)
            {
                sb = new StringBuilder();
                sw = new StringWriter(sb);
                tw = new HtmlTextWriter(sw);
                output = (HttpWriter)filterContext.RequestContext.HttpContext.Response.Output;
                filterContext.RequestContext.HttpContext.Response.Output = tw;
            }
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (sb == null)
            {
                return;
            }
            if (filterContext.Exception != null)
            {
                filterContext.RequestContext.HttpContext.Response.Output = output;
                base.OnResultExecuted(filterContext);
                return;
            }
            if (filterContext.HttpContext.Items.DocTransforms().Any())
            {
                string response = sb.ToString();
                //response processing

                var doc = new CsQuery.CQ(response);
                try
                {
                    foreach (var transform in filterContext.HttpContext.Items.DocTransforms())
                    {
                        transform(doc);
                    }
                    output.Write(doc.Render());
                }
                catch (Exception ex)
                {
                    output.Write(ex.ToString());
                    throw;
                }
            }
            base.OnResultExecuted(filterContext);
        }
    }

    public static class DocumentResultMods
    {
        public static void AddDocTransform(this IDictionary items, Action<CQ> transform)
        {
            var transforms = DocTransforms(items);
            transforms.Add(cq =>
            {
                transform(cq);
                return cq;
            });
        }
        public static void AddDocTransform(this IDictionary items, Func<CQ, CQ> transform)
        {
            DocTransforms(items).Add(transform);
        }

        public static List<Func<CQ, CQ>> DocTransforms(this IDictionary items)
        {
            if (!items.Contains("DocTransforms"))
            {
                items["DocTransforms"] = new List<Func<CQ, CQ>>();
            }
            return (List<Func<CQ, CQ>>)items["DocTransforms"];
        }
        public static bool SuppressDocTransforms(this IDictionary items, bool? value = null)
        {
            if (value == null)
            {
                return items["DocTransforms"] as bool? ?? false;
            }
            items["DocTransforms"] = value;
            return value.Value;
        }

    }
}