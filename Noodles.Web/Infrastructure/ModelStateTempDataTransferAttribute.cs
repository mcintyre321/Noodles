using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Noodles.Infrastructure
{
    public class ModelStateTempDataTransferAttribute : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTempDataTransferAttribute).FullName;
    
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            //Only export when ModelState is not valid
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                //Export if we are redirecting
                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                {
                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnResultExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

            if (modelState != null)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    ((ViewResult)filterContext.Result).ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    //Otherwise remove it.
                    //filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
