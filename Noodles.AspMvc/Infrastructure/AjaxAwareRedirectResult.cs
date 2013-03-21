using System.Web.Mvc;
using Noodles.RequestHandling.ResultTypes;

namespace Noodles.AspMvc.Infrastructure
{
    public class AjaxAwareRedirectResult : ActionResult
    {
        private readonly RedirectResult _inner;

        public AjaxAwareRedirectResult(RedirectResult inner)
        {
            _inner = inner;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                context.RequestContext.HttpContext.Response.AddHeader("Location", _inner.Url);
                context.RequestContext.HttpContext.Response.AddHeader("IsAjaxRedirect", "true");
            }
            else
            {
                _inner.ExecuteResult(context);
            }

        }
    }
}