using System.Web.Mvc;

namespace Noodles.AspMvc.Infrastructure
{
    public class AjaxAwareRedirectResult : ActionResult
    {
        private string url;

        public AjaxAwareRedirectResult(string url)
        {
            this.url = url;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                context.RequestContext.HttpContext.Response.AddHeader("Location", url);
                context.RequestContext.HttpContext.Response.AddHeader("IsAjaxRedirect", "true");
            }
            else
            {
                new RedirectResult(url).ExecuteResult(context);
            }   

        }
    }
}