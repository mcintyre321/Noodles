using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Mvc.JQuery.Datatables;
using WebNoodle.Reflection;

namespace WebNoodle
{
    public class NoodleResultBuilder
    {
        //public static List<Func<Exception, ModelStateDictionary, Action>> ModelStateExceptionHandlers =
        //    new List<Func<Exception, ModelStateDictionary, Action>>();
        public ActionResult Execute(ControllerContext cc, object node, Action<object, IObjectMethod, object[]> doInvoke = null)
        {
            doInvoke = doInvoke ?? (DoInvoke);

            if (cc.HttpContext.Request.HttpMethod.ToLower() == "get" || cc.HttpContext.Request.QueryString["action"] == null)
            {
                if (cc.HttpContext.Request.QueryString["action"] == "getNodeActions")
                {
                    var result = new PartialViewResult {ViewName = @"WebNoodle/NodeActions", ViewData = {Model = node}};
                    return result;
                }
                
                var viewname = FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType())
                    ?? FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType(), null, t => t.Name);
                var vr = new ViewResult {ViewName = viewname, ViewData = cc.Controller.ViewData };
                vr.ViewData.Model = node;
                return vr;
            }
            else //must be a post
            {
                using (Profiler.Step("Post"))
                {
                    if (cc.HttpContext.Request.QueryString["action"] == "getDataTable")
                    {
                        var propertyName = cc.HttpContext.Request.QueryString["prop"];
                        var queryable = node.GetType().GetProperty(propertyName).GetGetMethod().Invoke(node, null);
                        var dtr = Create(queryable, BindObject<DataTablesParam>(cc, "dataTableParam"));
                        return dtr;
                    }
                    {
                        var methodInstance = node.NodeMethods().Single(m => m.Name == cc.HttpContext.Request.QueryString["action"]);
                        var parameters = methodInstance.Parameters.Select(pt => this.BindObject(cc, pt.BindingParameterType, /*node.Id + "_" + methodInstance.Name + "_" +*/ pt.Name)).ToArray();
                        var msd = cc.Controller.ViewData.ModelState;
                        if (msd.IsValid)
                        {
                            try
                            {
                                doInvoke(node, methodInstance, parameters);
                            }
                            catch (TargetInvocationException ex)
                            {
                                msd.AddModelError("", ex.InnerException ?? ex);
                            }
                            catch (Exception ex)
                            {
                                msd.AddModelError("", ex);
                            }

                        }
                        if (cc.HttpContext.Request.IsAjaxRequest())
                        {
                            if (!msd.IsValid)
                            {
                                var res = new PartialViewResult
                                {
                                    ViewName = "WebNoodle/NodeActionForm",
                                    ViewData = {Model = methodInstance},
                                };
                                res.ViewData.ModelState.Merge(msd);
                                return res;
                            }
                            else
                            {
                                var res = new PartialViewResult
                                {
                                    ViewName = "WebNoodle/NodeActionSuccess",
                                    ViewData = { Model = methodInstance },
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

        public static DataTablesResult Create(object queryable, DataTablesParam dataTableParam)
        {
            try
            {
                var openCreateMethod =
                    typeof(DataTablesResult).GetMethods().Single(x => x.Name == "Create" && x.GetGenericArguments().Count() == 1);
                var queryableType = queryable.GetType().GetGenericArguments()[0];
                var closedCreateMethod = openCreateMethod.MakeGenericMethod(queryableType);
                return (DataTablesResult)closedCreateMethod.Invoke(null, new [] { queryable, dataTableParam});
            }
            catch (Exception ex)
            {
                throw new Exception("Was the object passed in a Something<T>?", ex);
            }
        }

        private static void DoInvoke(object node, IObjectMethod methodInstance, object[] parameters)
        {
            methodInstance.Invoke(parameters);
        }

        private T BindObject<T>(ControllerContext cc, string name) where T : class
        {
            return BindObject(cc, typeof (T), name) as T;
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