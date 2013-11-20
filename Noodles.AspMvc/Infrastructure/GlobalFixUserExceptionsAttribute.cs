using System.Linq;
using System.Web.Mvc;

namespace Noodles.AspMvc.Infrastructure
{
    public class GlobalFixUserExceptionsAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var ms = filterContext.Controller.ViewData.ModelState;
            if (!ms.IsValid)
            {
                var userErrors = (
                                     from modelStateItem in ms
                                     from e in modelStateItem.Value.Errors
                                     where e.Exception != null
                                     let ex = (e.Exception.InnerException ?? e.Exception) as UserException
                                     where string.IsNullOrEmpty(e.ErrorMessage) && ex != null
                                     select new { m = modelStateItem, e, ex }).ToArray();

                foreach (var error in userErrors)
                {
                    error.m.Value.Errors.Remove(error.e);
                }
                foreach (var error in userErrors)
                {
                    var key = (string.IsNullOrWhiteSpace(error.m.Key) ? error.ex.MemberName ?? "" : error.m.Key);
                    ms.AddModelError(key, error.ex.Message);
                }
            }
        }
    }
}