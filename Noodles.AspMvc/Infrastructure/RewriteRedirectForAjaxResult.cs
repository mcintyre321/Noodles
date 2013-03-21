using System.Web.Mvc;
using Noodles.RequestHandling.ResultTypes;

namespace Noodles.AspMvc.Infrastructure
{
    public class RewriteRedirectForAjaxResult : ActionResult
    {
        private readonly ActionResult _inner;

        public RewriteRedirectForAjaxResult(ActionResult inner)
        {
            _inner = inner;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                _inner.ExecuteResult(context);
                if (context.RequestContext.HttpContext.Response.StatusCode == 302)
                {
                    context.RequestContext.HttpContext.Response.AddHeader("IsAjaxRedirect", "true");
                }
            }
            else
            {
                _inner.ExecuteResult(context);
            }

        }
    }
}