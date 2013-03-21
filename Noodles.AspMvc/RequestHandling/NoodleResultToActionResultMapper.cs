using System.Net;
using System.Web.Mvc;
using Noodles.AspMvc.Infrastructure;
using Noodles.Models;
using Noodles.RequestHandling;
using Noodles.RequestHandling.ResultTypes;
using ViewResult = Noodles.RequestHandling.ResultTypes.ViewResult;

namespace Noodles.AspMvc.RequestHandling
{
    public class NoodleResultToActionResultMapper : NoodleResultMapper<ActionResult, ControllerContext>
    {
        public override ActionResult Map(ControllerContext context, BadRequestResult result)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public override ActionResult Map(ControllerContext context, ErrorResult result)
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public override ActionResult Map(ControllerContext context, NotFoundResult result)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public override ActionResult Map(ControllerContext context, ValidationErrorResult result)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var res = new System.Web.Mvc.ViewResult();
            res.ViewName = "Noodles/NodeMethod";
            res.ViewData.Model = result.Invokeable;
            res.ViewData.ModelState.Merge(context.Controller.ViewData.ModelState);
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Noodles/_AjaxLayout";
            }
            return res;
        }

        public override ActionResult Map(ControllerContext context, ViewResult result)
        {
            var res = new System.Web.Mvc.ViewResult();
            if (result.Target is Resource)
            {
                var resource = (Resource)result.Target;
                var viewname = FormFactory.FormHelperExtension.BestViewName(context, resource.ValueType, "Noodles/Node.");
                res.ViewName = viewname;
            }
            else if (result.Target is IInvokeable)
            {
                res.ViewName = "Noodles/NodeMethod";
            }
            res.ViewData.Model = result.Target;
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Noodles/_AjaxLayout";
            }
            return res;
        }

        public override ActionResult Map(ControllerContext context, InvokeSuccessResult result)
        {
            if (result.Result is ActionResult)
            {
                if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    return new RewriteRedirectForAjaxResult((ActionResult) result.Result);
                }
                return (ActionResult) result.Result;
            }
            var res = new System.Web.Mvc.ViewResult();
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Noodles/_AjaxLayout";
            }
            res.ViewName = "Noodles/NodeMethodSuccess";
            res.ViewData.Model = result;
            return res;
        }

    }
}