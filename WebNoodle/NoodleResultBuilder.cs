using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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
                    var result = new PartialViewResult() { ViewName = @"WebNoodle/NodeActions" };
                    result.ViewData.Model = node;
                    return result;
                }
                var viewname = FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType())
                    ?? FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType(), null, t => t.Name);
                var viewData = cc.Controller.ViewData;
                viewData.Model = node;
                var vr = new ViewResult {ViewName = viewname, ViewData = viewData};
                return vr;
            }
            else //must be a post
            {
                using (Profiler.Step("Post"))
                {
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


        private static void DoInvoke(object node, IObjectMethod methodInstance, object[] parameters)
        {
            methodInstance.Invoke(parameters);
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