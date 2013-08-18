using System.Web.Mvc;
using CsQuery;
using Noodles.AspMvc.Infrastructure;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class SelectFragmentTransform : IDocumentTransform
    {
        public void Register(ControllerContext cc, INode resource)
        {
            var fragmentSelector = cc.RequestContext.HttpContext.Request["fragment-selector"];
            if (fragmentSelector != null)
            {
                cc.HttpContext.Items.AddTransform(cq => CQ.CreateFragment((string) cq.Document.QuerySelector(fragmentSelector).OuterHTML));
            }
        }
    }
}