using System.Web.Mvc;

namespace Noodles.AspMvc.RequestHandling
{
    public class AjaxRedirectRewritingActionResult : ActionResult
    {
        private readonly ActionResult _inner;

        public AjaxRedirectRewritingActionResult(ActionResult inner)
        {
            _inner = inner;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var redirectResult = _inner as RedirectResult;
            if (redirectResult != null && context.HttpContext.Request.IsAjaxRequest())
            {
                context.HttpContext.Response.AddHeader("Location", redirectResult.Url);
                context.HttpContext.Response.AddHeader("IsAjaxRedirect", "true");

            }
            else
            {
                _inner.ExecuteResult(context);
            }
        }
    }
}