using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Noodles.Requests;
using Noodles.Requests.Results;
using ViewResult = Noodles.Requests.Results.ViewResult;

namespace Noodles.AspMvc.Requests
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
            context.HttpContext.Response.StatusCode = 409;
            var res = new System.Web.Mvc.ViewResult();
            res.ViewName = "Noodles/NodeMethod";
            res.ViewData.Model = result.Invokeable;
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
            else
            {
                res.ViewName = "Noodles/Node";
            }
            res.ViewData.Model = result.Node;
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Noodles/_AjaxLayout";
            } return res;
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