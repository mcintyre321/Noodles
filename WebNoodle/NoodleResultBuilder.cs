using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Mvc.JQuery.Datatables;

namespace WebNoodle
{
    public class NoodleResultBuilder
    {
        static List<Func<Exception, ModelStateDictionary, Action>> ModelStateExceptionHandlers = new List<Func<Exception, ModelStateDictionary, Action>>();
        public static void AddExceptionHandler<TEx>(Action<TEx, ModelStateDictionary> action) where TEx : Exception
        {
            ModelStateExceptionHandlers.Add((e, msd) => (e as TEx) == null ? null as Action : () => action((TEx) e, msd));
        }
        static NoodleResultBuilder()
        {
            AddExceptionHandler<UserException>((e, msd) => msd.AddModelError("", e));
        }
        public ActionResult Execute(ControllerContext cc, object node, Action<object, IObjectMethod, object[]> doInvoke = null)
        {
            doInvoke = doInvoke ?? (DoInvoke);

            if (cc.HttpContext.Request.HttpMethod.ToLower() == "get" || cc.HttpContext.Request.QueryString["action"] == null)
            {
                if (cc.HttpContext.Request.QueryString["action"] == "getNodeMethods")
                {
                    using (Profiler.Step("Getting node actions"))
                    {
                        var result = new PartialViewResult { ViewName = @"WebNoodle/NodeMethods", ViewData = { Model = node } };
                        return result;
                    }
                }
                var nodeAction = node.NodeMethods().SingleOrDefault(m => cc.HttpContext.Request.QueryString["action"] == m.Name);

                if (nodeAction != null)
                {
                    using (Profiler.Step("Getting node action"))
                    {
                        var result = new PartialViewResult { ViewName = @"WebNoodle/NodeMethod", ViewData = { Model = nodeAction } };
                        return result;
                    }
                }

                using (Profiler.Step("Returning view"))
                {
                    var viewname = FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType())
                                   ??
                                   FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType(), null, t => t.Name);
                    var vr = new ViewResult { ViewName = viewname, ViewData = cc.Controller.ViewData };
                    vr.ViewData.Model = node;
                    return vr;
                }
            }
            else //must be a post
            {
                using (Profiler.Step("Post"))
                {
                    if (cc.HttpContext.Request.QueryString["action"] == "getDataTable")
                    {
                        using (Profiler.Step("Returning DataTable"))
                        {
                            var propertyName = cc.HttpContext.Request.QueryString["prop"];
                            var queryable = node.GetType().GetProperty(propertyName).GetGetMethod().Invoke(node, null);
                            var transformKey = cc.HttpContext.Request.QueryString["transform"];
                            if (transformKey != null)
                            {
                                dynamic transform = cc.HttpContext.Cache[transformKey];
                                queryable = transform.Invoke(queryable);
                            }
                            var dtr = Mvc.JQuery.Datatables.DataTablesResult.Create(queryable, BindObject<DataTablesParam>(cc, "dataTableParam"));
                            return dtr;
                        }
                    }
                    {
                        using (Profiler.Step("Executing action " + cc.HttpContext.Request.QueryString["action"]))
                        {
                            var methodInstance = node.NodeMethods().Single(m => m.Name == cc.HttpContext.Request.QueryString["action"]);
                            var parameters = methodInstance.Parameters.Select(pt => this.BindObject(cc, pt.BindingParameterType, pt.Name)).ToArray();
                            var msd = cc.Controller.ViewData.ModelState;
                            if (msd.IsValid)
                            {
                                Logger.Trace("ModelBinding successful");
                                try
                                {
                                    doInvoke(node, methodInstance, parameters);
                                    Logger.Trace("Invoke successful");
                                }
                                catch (Exception ex)
                                {
                                    if (ex is TargetInvocationException)
                                    {
                                        ex = ex.InnerException ?? ex;
                                    }
                                    Action handle = ModelStateExceptionHandlers.Select(h => h(ex, msd)).FirstOrDefault(h => h != null);

                                    if (handle != null)
                                    {
                                        handle();
                                        cc.HttpContext.Response.StatusCode = 409;
                                    }
                                    else
                                    {
                                        msd.AddModelError("", "An error occurred");
                                        cc.HttpContext.Response.StatusCode = 500;
                                        Logger.LogError(ex);
                                    }
                                }
                            }
                            if (cc.HttpContext.Request.IsAjaxRequest())
                            {
                                Logger.Trace("In ajax request");
                                if (!msd.IsValid)
                                {
                                    var res = new PartialViewResult
                                    {
                                        ViewName = "WebNoodle/NodeMethod",
                                        ViewData = {Model = methodInstance},
                                    };
                                    res.ViewData.ModelState.Merge(msd);
                                    return res;
                                }
                                else
                                {
                                    Logger.Trace("Returning success");
                                    var res = new PartialViewResult
                                    {
                                        ViewName = "WebNoodle/NodeMethodSuccess",
                                        ViewData = {Model = methodInstance},
                                    };

                                    res.ViewData.ModelState.Merge(msd);
                                    return res;
                                }
                            }
                            else
                            {
                                return new RedirectResult(cc.HttpContext.Request.UrlReferrer.ToString());
                            }
                        }
                    }
                }
            }
        }


        private static void DoInvoke(object node, IObjectMethod methodInstance, object[] parameters)
        {
            methodInstance.Invoke(parameters);
        }

        private T BindObject<T>(ControllerContext cc, string name) where T : class
        {
            return BindObject(cc, typeof(T), name) as T;
        }
        private object BindObject(ControllerContext cc, Type desiredType, string name)
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