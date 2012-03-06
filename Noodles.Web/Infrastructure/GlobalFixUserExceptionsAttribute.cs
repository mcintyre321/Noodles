using System.Linq;
using System.Web.Mvc;

namespace Noodles.Infrastructure
{
    public class GlobalFixUserExceptionsAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var ms = filterContext.Controller.ViewData.ModelState;
            if (!ms.IsValid)
            {
                var userErrors = (
                                     from m in ms
                                     from e in m.Value.Errors
                                     where e.Exception != null
                                     let ex = (e.Exception.InnerException ?? e.Exception) as UserException
                                     where string.IsNullOrEmpty(e.ErrorMessage) && ex != null
                                     select new { m, e, ex }).ToArray();

                foreach (var error in userErrors)
                {
                    error.m.Value.Errors.Remove(error.e);
                }
                foreach (var error in userErrors)
                {
                    ms.AddModelError(error.m.Key, error.ex.Message);
                }
            }
        }
    }
}