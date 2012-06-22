using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using Walkies;

namespace Noodles
{
    public static class ActionResultExtension
    {
        public static List<Func<ControllerContext, object, ActionResult>> Processors = new List<Func<ControllerContext, object, ActionResult>>();
        static List<Func<Exception, ControllerContext, Action>> ModelStateExceptionHandlers = new List<Func<Exception, ControllerContext, Action>>();
        public static void AddExceptionHandler<TEx>(Action<TEx, ControllerContext> action) where TEx : Exception
        {
            ModelStateExceptionHandlers.Add((e, msd) => (e as TEx) == null ? null as Action : () => action((TEx)e, msd));
            ModelStateExceptionHandlers.Add((e, msd) => (e as NodeNotFoundException) == null ? null as Action : () => action((TEx)e, msd));

            Walkies.WalkExtension.Rules.Add((o, fragment) => fragment == "actions" ? new NodeMethods(o) : null);
        }

        static ActionResultExtension()
        {
            AddExceptionHandler<UserException>((e, cc) => cc.Controller.ViewData.ModelState.AddModelError("", e));
        }

        private static ActionResult ProcessNodeMethodCall(ControllerContext cc, object node, Func<NodeMethod, object[], object> doInvoke)
        {
            var method = node as NodeMethod;
            if (method == null) return null;

            var httpMethod = cc.HttpContext.Request.HttpMethod;
            var isInvoke = httpMethod == "POST" || (httpMethod == "GET" && method.GetAttribute<HttpGetAttribute>() != null);
            if (!isInvoke) return null;
            var parameters = method.Parameters
                .Select(pt => pt.Locked ? pt.Value : BindObject(cc, pt.BindingParameterType, pt.Name))
                .ToArray();
            var msd = cc.Controller.ViewData.ModelState;
            if (msd.IsValid)
            {
                Logger.Trace("ModelBinding successful");
                try
                {
                    var result = doInvoke(method, parameters) as ActionResult;
                    if (result != null) return result;
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException)
                    {
                        ex = ex.InnerException ?? ex;
                    }
                    Action handle = ModelStateExceptionHandlers.Select(h => h(ex, cc)).FirstOrDefault(h => h != null);

                    if (handle != null)
                    {
                        cc.HttpContext.Response.StatusCode = 409;
                        handle();
                    }
                    else
                    {
                        cc.HttpContext.Response.StatusCode = 500;
                        throw;
                    }
                }
            }
            else
            {
                cc.HttpContext.Response.StatusCode = 409;
            }

            cc.HttpContext.Response.TrySkipIisCustomErrors = true;

            var nodeMethodReturnUrl = cc.RequestContext.HttpContext.Request["nodeMethodReturnUrl"];
            if (!cc.HttpContext.Request.IsAjaxRequest() && msd.IsValid)
            {
                return new RedirectResult(nodeMethodReturnUrl);
            }

            var viewName = msd.IsValid ? "Noodles/NodeMethodSuccess" : "Noodles/NodeMethod";
            ViewResultBase res;
            if (cc.HttpContext.Request.IsAjaxRequest())
            {
                res = new PartialViewResult
                {
                    ViewName = viewName,
                    ViewData = { Model = method },
                };
            }
            else
            {
                res = new ViewResult
                {
                    ViewName = viewName,
                    ViewData = { Model = method },
                };
            }
            res.ViewData.ModelState.Merge(msd);
            res.ViewData["nodeMethodReturnUrl"] = nodeMethodReturnUrl;
            return res;
        }

        public static ActionResult GetNoodleResult(this ControllerContext cc, object root, string path = null, Func<NodeMethod, object[], object> doInvoke = null)
        {

            path = path ?? cc.RouteData.Values["path"] as string ?? "/";
            object node = null;
            try
            {
                node = root.Walk(path.Trim('/')).Last();
            }
            catch (NodeNotFoundException ex)
            {
                return new HttpNotFoundResult(ex.Message);
            }
            if (node == null) return new HttpNotFoundResult();

            var processorResult = Processors.Select(p => p(cc, node)).FirstOrDefault(r => r != null)
                                  ?? ProcessNodeMethodCall(cc, node, doInvoke ?? DoInvoke)
                                  ?? ProcessGet(cc, node);
            if (processorResult != null) return processorResult;


            var httpMethod = cc.HttpContext.Request.HttpMethod;

            ActionResult noodleResult;
            return new HttpStatusCodeResult((int)System.Net.HttpStatusCode.BadRequest);
        }

        private static ActionResult ProcessGet(ControllerContext cc, object node)
        {
            if (cc.RequestContext.HttpContext.Request.HttpMethod == "GET")
            {
                using (Profiler.Step("Returning view"))
                {
                    var viewname = typeof(NodeMethod).IsAssignableFrom(node.NodeType())
                                       ? "Noodles/NodeMethod"
                                       : typeof(NodeMethods).IsAssignableFrom(node.NodeType())
                                             ? "Noodles/NodeMethods"
                                             : FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType()) ??
                                               FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType(), null,
                                                                                            t => t.Name);

                    var vr = new ViewResult { ViewName = viewname, ViewData = cc.Controller.ViewData };
                    if (cc.HttpContext.Request.IsAjaxRequest())
                    {
                        vr.MasterName = "Noodles/_AjaxLayout";
                    }

                    vr.ViewData.Model = node;
                    if (cc.HttpContext.Request.UrlReferrer != null)
                    {
                        vr.ViewData["nodeMethodReturnUrl"] = cc.HttpContext.Request.UrlReferrer.AbsolutePath;
                    }
                    return vr;
                }
            }
            return null;
        }


        private static object DoInvoke(NodeMethod nodeMethod, object[] parameters)
        {
            return nodeMethod.Invoke(parameters);
        }

        public static T BindObject<T>(ControllerContext cc, string name) where T : class
        {
            return BindObject(cc, typeof(T), name) as T;
        }
        private static object BindObject(ControllerContext cc, Type desiredType, string name)
        {

            var formCollection = cc.HttpContext.Request.Unvalidated().Form;

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);

            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, desiredType);
            var bindingContext = new ModelBindingContext
            {
                ModelName = name,
                ValueProvider = valueProvider,
                ModelMetadata = metadata,
                ModelState = cc.Controller.ViewData.ModelState
            };

            var binder = ModelBinders.Binders.GetBinder(desiredType, true);
            var output = binder.BindModel(cc, bindingContext);

            return output;
        }
    }
}