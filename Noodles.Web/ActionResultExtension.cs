using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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

        public static ActionResult GetNoodleResult(this ControllerContext cc, object root, string path = null, Func<NodeMethod, object[], object> doInvoke = null)
        {
            doInvoke = doInvoke ?? (DoInvoke);

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

            var processorResult = Processors.Select(p => p(cc, node)).FirstOrDefault(r => r != null);
            if (processorResult != null) return processorResult;

            if (cc.HttpContext.Request.HttpMethod.ToLowerInvariant() == "get")
            {
                using (Profiler.Step("Returning view"))
                {
                    var viewname = typeof(NodeMethod).IsAssignableFrom(node.NodeType()) ? "Noodles/NodeMethod" :
                                   typeof(NodeMethods).IsAssignableFrom(node.NodeType()) ? "Noodles/NodeMethods" :
                                   FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType()) ??
                                   FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType(), null, t => t.Name);

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
            else //must be a post
            {
                using (Profiler.Step("Post"))
                {
                    
                    {
                        var method = (NodeMethod)node;

                        using (Profiler.Step("Executing action " + method.Name))
                        {
                            var parameters = method.Parameters
                                .Select(pt => pt.Locked ? pt.Value : BindObject(cc, pt.BindingParameterType, pt.Name))
                                .ToArray();
                            var msd = cc.Controller.ViewData.ModelState;
                            if (msd.IsValid)
                            {
                                Logger.Trace("ModelBinding successful");
                                try
                                {
                                    var result = doInvoke(method, parameters);
                                    if (result is ActionResult) { return (ActionResult)result; }
                                    Logger.Trace("Invoke successful");
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

                            if (cc.HttpContext.Request.IsAjaxRequest())
                            {
                                Logger.Trace("In ajax request");
                                if (!msd.IsValid)
                                {
                                    var res = new PartialViewResult
                                                  {
                                                      ViewName = "Noodles/NodeMethod",
                                                      ViewData = { Model = method },
                                                  };
                                    res.ViewData.ModelState.Merge(msd);
                                    return res;
                                }
                                else
                                {
                                    Logger.Trace("Returning success");
                                    var res = new PartialViewResult
                                                  {
                                                      ViewName = "Noodles/NodeMethodSuccess",
                                                      ViewData = { Model = method },
                                                  };

                                    res.ViewData.ModelState.Merge(msd);
                                    return res;
                                }
                            }
                            else
                            {
                                Logger.Trace("Not ajax request");

                                var nodeMethodReturnUrl = cc.RequestContext.HttpContext.Request["nodeMethodReturnUrl"];

                                if (!msd.IsValid)
                                {
                                    var res = new ViewResult
                                    {
                                        ViewName = "Noodles/NodeMethod",
                                        ViewData = { Model = method },
                                    };
                                    res.ViewData.ModelState.Merge(msd);
                                    res.ViewData["nodeMethodReturnUrl"] = nodeMethodReturnUrl;

                                    return res;
                                }
                                else
                                {
                                    Logger.Trace("Returning success");

                                    if (nodeMethodReturnUrl != null)
                                    {
                                        return new RedirectResult(nodeMethodReturnUrl);
                                    }
                                    else
                                    {
                                        var res = new ViewResult
                                        {
                                            ViewName = "Noodles/NodeMethodSuccess",
                                            ViewData = { Model = method },
                                        };

                                        res.ViewData.ModelState.Merge(msd);
                                        return res;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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