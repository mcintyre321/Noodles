using System.Linq;
using System.Net;
using System.Web.Mvc;
using FormFactory;
using FormFactory.AspMvc.Wrappers;
using Noodles.AspMvc.Helpers;
using Noodles.AspMvc.Infrastructure;
using Noodles.AspMvc.RequestHandling.Transforms;
using Noodles.Models;
using Noodles.RequestHandling;
using Noodles.RequestHandling.ResultTypes;
using ViewResult = Noodles.RequestHandling.ResultTypes.ViewResult;

namespace Noodles.AspMvc.RequestHandling
{
    public class NoodleResultToActionResultMapper : NoodleResultMapper<ActionResult, ControllerContext>
    {
        private TransformRuleRegistry ruleRegistry;

        public NoodleResultToActionResultMapper(TransformRuleRegistry ruleRegistry)
        {
            this.ruleRegistry = ruleRegistry;
        }

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
            foreach (var error in result)
            {
                context.Controller.ViewData.ModelState.AddModelError(error.Key, error.Value);
            }
            return BuildActionResult(context, (INode) result.Invokeable);
        }

        public override ActionResult Map(ControllerContext context, ViewResult result)
        {
            return BuildActionResult(context, result.Target);
        }

        private ActionResult BuildActionResult(ControllerContext context, INode targetResource)
        {
            var res = new System.Web.Mvc.ViewResult();
            if (targetResource != null)
            {
                var jsonTypeHeader = context.HttpContext.Request.AcceptTypes.FirstOrDefault(x => x.EndsWith("json"));
                if (jsonTypeHeader != null)
                {
                    // eg application/vnd.mycompany.mydtotype-v1+json
                    jsonTypeHeader = jsonTypeHeader.RemoveFromStart("application/", true).RemoveFromEnd("+json", true);
                    var converter = ContentConverters.Get(jsonTypeHeader);
                    if (converter != null)
                    {
                        var convertedTarget = converter(targetResource);
                        return new JsonResult { Data = convertedTarget };
                    }
                }
             

                var ffContext = (FormFactory.IViewFinder) new FormFactoryContext(context);
                var viewname = ViewFinderExtensions.BestViewName(ffContext, targetResource.ValueType, "Noodles/Node.")
                                ?? "Noodles/Node.Object";

                context.HttpContext.Items.SetDocTransformsEnabled(true);
                res.ViewName = viewname;
                context.Controller.ViewBag.NoodleTarget = targetResource;
                ruleRegistry.RegisterTransformations(context, targetResource);
            }
            res.ViewData.ModelState.Merge(context.Controller.ViewData.ModelState);
            if (!res.ViewData.ModelState.IsValid)
            {
                context.HttpContext.Response.TrySkipIisCustomErrors = true;
                context.HttpContext.Response.StatusCode = 400;
            }
            res.ViewData.Model = targetResource;
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                res.MasterName = "Noodles/_AjaxLayout";
            }
            return res;
        }

        public override ActionResult Map(ControllerContext context, InvokeSuccessResult result)
        {
            var redirectResult = result.Result as RedirectResult;
            var request = context.RequestContext.HttpContext.Request;
            if (redirectResult != null && request.IsAjaxRequest())
            {
                return new AjaxRedirectRewritingActionResult((ActionResult) result.Result);
            }
            var actionResult = result.Result as ActionResult;
            if (actionResult != null)
            {
                return actionResult;
            }

            if (request.AcceptTypes.Contains("application/json"))
            {
                return new JsonResult() { Data = result.Result };
            }

            if (!request.IsAjaxRequest() && request.UrlReferrer != null)
            {
                return new RedirectResult(request.UrlReferrer.ToString());
            }
            var targetResource = result.Invokeable as INode;
            return BuildActionResult(context, targetResource);
        }

    }
}