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


        public ActionResult Execute(ControllerContext cc, INode node, Action<INode, IObjectMethod, object[]> doInvoke = null)
        {
            doInvoke = doInvoke ?? (DoInvoke);

            if (cc.HttpContext.Request.HttpMethod.ToLower() == "get")
            {
                if (cc.HttpContext.Request.QueryString["action"] == "getNodeActions")
                {
                    var result = new PartialViewResult() {ViewName = @"WebNoodle/NodeActions"};
                    result.ViewData.Model = node;
                    return result;
                }

                var vr = new ViewResult() {ViewName = "WebNoodle/Node"};
                vr.ViewData.Model = node;
                return vr;
            }
            else //must be a post
            {
                using (Profiler.Step("Post"))
                {
                    {
                        var methodInstance = node.NodeMethods().Single(m => m.Name == cc.HttpContext.Request.QueryString["action"]);
                        var parameters = methodInstance.Parameters.Select(pt => this.BindObject(cc, pt.BindingParameterType, node.Id + "_" + methodInstance.Name + "_" + pt.Name)).ToArray();
                        if (cc.Controller.ViewData.ModelState.IsValid)
                        {
                            doInvoke(node, methodInstance, parameters);
                        }
                        else
                        {
                            var errors = cc.Controller.ViewData.ModelState.Values.SelectMany(v => v.Errors);
                            var messages = errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                            throw new UserException("There were some errors: \r\n" + string.Join("\r\n", messages));
                        }
                        return new RedirectResult(cc.HttpContext.Request.UrlReferrer.ToString());

                        //return ActionInvoker.InvokeAction()
                        //else
                        //{
                        //    var target = _noodleTarget.FetchReadonly().YieldChildren(path, true).Last();
                        //    return View()
                        //}

                        //if (Request.IsAjaxRequest())
                        //{
                        //    var node = _sourcerer.ReadModel.YieldPath(fixedPath).Last();
                        //    return PartialView(node.GetType().Name, node);
                        //}
                        //else
                        //{
                        //    var page = _sourcerer.ReadModel.YieldPath(fixedPath, true).OfType<IPage>().Last();
                        //    return Redirect(page.Path);
                        //}

                    }
                }
            }
        }

        private static void DoInvoke(INode node, IObjectMethod methodInstance, object[] parameters)
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