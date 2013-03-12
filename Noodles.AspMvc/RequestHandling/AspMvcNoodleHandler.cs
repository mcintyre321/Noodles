using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FormFactory;
using Noodles.Models;
using Noodles.RequestHandling;

namespace Noodles.AspMvc.RequestHandling
{
    public class AspMvcNoodleHandler : Handler<ControllerContext>
    {
        public static List<Func<ControllerContext, object, ActionResult>> Processors = new List<Func<ControllerContext, object, ActionResult>>();
        static List<Func<Exception, ControllerContext, Action>> ModelStateExceptionHandlers = new List<Func<Exception, ControllerContext, Action>>();

        public static void AddExceptionHandler<TEx>(Action<TEx, ControllerContext> action) where TEx : Exception
        {
            ModelStateExceptionHandlers.Add((e, msd) => (e as TEx) == null ? null as Action : () => action((TEx)e, msd));
            ModelStateExceptionHandlers.Add((e, msd) => (e as NodeNotFoundException) == null ? null as Action : () => action((TEx)e, msd));
        }

        static AspMvcNoodleHandler()
        {
            //AddExceptionHandler<UserException>((e, cc) => cc.Controller.ViewData.ModelState.AddModelError("", e));
            var propertyVms = VmHelper.GetPropertyVms;
            VmHelper.GetPropertyVms = (h, o, a) => GetPropertyVms(h, o, a, propertyVms);
        }


        private static IEnumerable<PropertyVm> GetPropertyVms(HtmlHelper htmlHelper, object o, Type type, Func<HtmlHelper, object, Type, IEnumerable<PropertyVm>> fallback)
        {
            if (o == null) o = Activator.CreateInstance(type);
            return o.YieldFindPropertyInfosUsingReflection(type).Select(p => new PropertyVm(o, p, htmlHelper));
        }

        public async Task<ActionResult> GetNoodleResult(ControllerContext cc, object root, string path = null, Func<IInvokeable, object[], object> doInvoke = null)
        {
            root.SetUrlRoot(cc);

            var nr = new AspMvcRequestInfo(cc);
            var handler = new AspMvcNoodleHandler();
            var pathParts = (path ?? cc.RouteData.Values["path"] as string ?? "/").Trim('/')
                .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var noodleResponse = await handler.HandleRequest(cc, nr, root, pathParts, doInvoke);

            var transformer = new NoodleResultToActionResultMapper();
            return transformer.Map(cc, noodleResponse);
        }

        //private static ActionResult ProcessGet(ControllerContext cc, object node)
        //{
        //    if (node is ActionResult) return (ActionResult) node;
        //    if (cc.RequestContext.HttpContext.Request.HttpMethod == "GET")
        //    {
        //        using (Profiler.Step("Returning view"))
        //        {
        //            var viewname = typeof(NodeMethod).IsAssignableFrom(node.NodeType())
        //                               ? "Noodles/NodeMethod"
        //                               : typeof(NodeMethodsReflectionLogic).IsAssignableFrom(node.NodeType())
        //                                     ? "Noodles/NodeMethods"
        //                                     : FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType());

        //            var vr = new NoodleViewResult { ViewName = viewname, ViewData = cc.Controller.ViewData };
        //            if (cc.HttpContext.Request.IsAjaxRequest())
        //            {
        //                vr.MasterName = "Noodles/_AjaxLayout";
        //            }

        //            vr.ViewData.Model = node;
        //            if (cc.HttpContext.Request.UrlReferrer != null)
        //            {
        //                vr.ViewData["nodeMethodReturnUrl"] = cc.HttpContext.Request.UrlReferrer.AbsolutePath;
        //            }
        //            return vr;
        //        }
        //    }
        //    return null;
        //}




    }
}