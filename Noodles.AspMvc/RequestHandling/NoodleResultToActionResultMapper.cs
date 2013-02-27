using System.Net;
using System.Web.Mvc;
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
            if (result.Node is IInvokeable)
            {
                res.ViewName = "Noodles/NodeMethod";
            }
            else if (result.Node is Resource)
            {
                var resource = (Resource)result.Node;
                var viewname = FormFactory.FormHelperExtension.BestViewName(context, resource.Type, "Noodles/Node.");
                res.ViewName = viewname;
            }
            res.ViewData.Model = result.Node;
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Noodles/_AjaxLayout";
            }
            return res;
        }

        public override ActionResult Map(ControllerContext context, InvokeSuccessResult result)
        {
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