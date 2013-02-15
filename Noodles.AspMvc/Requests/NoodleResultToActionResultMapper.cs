using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Noodles.Requests;
using Noodles.Requests.Results;

namespace Noodles.AspMvc.Requests
{
    public class NoodleResultToActionResultMapper : NoodleResultMapper<ActionResult, ControllerContext>
    {
        public override ActionResult Map(ControllerContext context, BadRequestResult result)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public override ActionResult Map(ControllerContext context, NoodlesErrorResult result)
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public override ActionResult Map(ControllerContext context, NotFoundResult result)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public override ActionResult Map(ControllerContext context, NoodlesValidationErrorResult result)
        {
            context.HttpContext.Response.StatusCode = 409;
            var res = new ViewResult();
            res.ViewName = "Noodles/NodeMethod";
            res.ViewData.Model = result.Invokeable;
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Shared/Noodles/_AjaxLayout.cshtml";
            }
            return res;
        }

        public override ActionResult Map(ControllerContext context, NoodlesViewResult result)
        {
            var res = new ViewResult();
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
            var res = new ViewResult();
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Shared/Noodles/_AjaxLayout.cshtml";
            }
            res.ViewName = "Noodles/NodeMethodSuccess";
            res.ViewData.Model = result.Invokeable;
            return res;
        }
    }
}