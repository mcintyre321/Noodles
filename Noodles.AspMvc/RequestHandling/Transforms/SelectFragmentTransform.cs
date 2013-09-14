using System.Web.Mvc;
using CsQuery;
using Noodles.AspMvc.Infrastructure;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class SelectFragmentTransformContext : ITransformContext
    {
        public void TransformContext(ControllerContext cc, INode parent)
        {
            var fragmentSelector = cc.RequestContext.HttpContext.Request["fragment-selector"];
            if (fragmentSelector != null)
            {
                cc.HttpContext.Items.AddDocTransform(cq => CQ.CreateFragment(cq.Document.QuerySelector(fragmentSelector).OuterHTML));
            }
        }
    }
}