using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using Noodles.Web.Models;
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

        }

        static ActionResultExtension()
        {
            AddExceptionHandler<UserException>((e, cc) => cc.Controller.ViewData.ModelState.AddModelError("", e));
            Noodles.Configuration.Initialise();
        }

        private static ActionResult ProcessNodeMethodCall(ControllerContext cc, object node, Func<NodeMethod, object[], object> doInvoke)
        {
            var method = node as NodeMethod;
            if (method == null) return null;

            var httpMethod = cc.HttpContext.Request.HttpMethod;
            var isInvoke = httpMethod == "POST" || (httpMethod == "GET" && method.GetAttribute<HttpGetAttribute>() != null);
            if (!isInvoke) return null;
            var parameters = method.Parameters
                .Select(pt => pt.Locked ? pt.Value : BindObject(cc, pt.ParameterType, pt.Name, pt.CustomAttributes, pt.DisplayName))
                .ToArray();
            var msd = cc.Controller.ViewData.ModelState;

            object result = null;
            if (msd.IsValid)
            {
                Logger.Trace("ModelBinding successful");
                try
                {
                    result = doInvoke(method, parameters);
                    if (result is ActionResult) return (ActionResult)result;
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

            ViewResultBase res = cc.HttpContext.Request.IsAjaxRequest() ? (ViewResultBase)new NoodlePartialViewResult() : new NoodleViewResult();
            if (msd.IsValid)
            {
                res.ViewName = "Noodles/NodeMethodSuccess";
                res.ViewData.Model = new NodeMethodSuccessVm(method, result);
            }
            else
            {
                res.ViewName = "Noodles/NodeMethod";
                res.ViewData.Model = method;
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


            return new HttpStatusCodeResult((int)System.Net.HttpStatusCode.BadRequest);
        }

        private static ActionResult ProcessGet(ControllerContext cc, object node)
        {
            if (node is ActionResult) return (ActionResult) node;
            if (cc.RequestContext.HttpContext.Request.HttpMethod == "GET")
            {
                using (Profiler.Step("Returning view"))
                {
                    var viewname = typeof(NodeMethod).IsAssignableFrom(node.NodeType())
                                       ? "Noodles/NodeMethod"
                                       : typeof(NodeMethodsReflectionLogic).IsAssignableFrom(node.NodeType())
                                             ? "Noodles/NodeMethods"
                                             : FormFactory.FormHelperExtension.BestViewName(cc, node.NodeType());

                    var vr = new NoodleViewResult { ViewName = viewname, ViewData = cc.Controller.ViewData };
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
            return BindObject(cc, typeof(T), name, null, null) as T;
        }
        private static object BindObject(ControllerContext cc, Type desiredType, string name, IEnumerable<Attribute> attributes, string displayName)
        {
            attributes = attributes ?? new Attribute[] {};
            displayName = displayName ?? name.Sentencise(true);
            var nameValueCollection = new NameValueCollection
            {
                cc.HttpContext.Request.Unvalidated().Form, cc.HttpContext.Request.QueryString
            };

            var valueProvider = new NameValueCollectionValueProvider(nameValueCollection, null);

            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, desiredType);
            metadata.DisplayName = displayName;
            ApplyAttributeMetaData(metadata, attributes);
            var bindingContext = new ModelBindingContext
            {
                ModelName = name,
                ValueProvider = valueProvider,
                ModelMetadata = metadata,
                ModelState = cc.Controller.ViewData.ModelState
            };

            
            var binder = ModelBinders.Binders.GetBinder(desiredType, true);
            var output = binder.BindModel(cc, bindingContext);

            foreach (var va in attributes.OfType<ValidationAttribute>())
            {
                if (!va.IsValid(output))
                {
                    bindingContext.ModelState.AddModelError(name, va.FormatErrorMessage(displayName));
                }
            }

            return output;
        }

        

        private static void ApplyAttributeMetaData(ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var attributeList = new List<Attribute>(attributes);
            DisplayColumnAttribute displayColumnAttribute = attributeList.OfType<DisplayColumnAttribute>().FirstOrDefault();
            var result = metadata;

            // Do [HiddenInput] before [UIHint], so you can override the template hint
            HiddenInputAttribute hiddenInputAttribute = attributeList.OfType<HiddenInputAttribute>().FirstOrDefault();
            if (hiddenInputAttribute != null)
            {
                result.TemplateHint = "HiddenInput";
                result.HideSurroundingHtml = !hiddenInputAttribute.DisplayValue;
            }

            // We prefer [UIHint("...", PresentationLayer = "MVC")] but will fall back to [UIHint("...")]
            IEnumerable<UIHintAttribute> uiHintAttributes = attributeList.OfType<UIHintAttribute>();
            UIHintAttribute uiHintAttribute = uiHintAttributes.FirstOrDefault(a => String.Equals(a.PresentationLayer, "MVC", StringComparison.OrdinalIgnoreCase))
                                           ?? uiHintAttributes.FirstOrDefault(a => String.IsNullOrEmpty(a.PresentationLayer));
            if (uiHintAttribute != null)
            {
                result.TemplateHint = uiHintAttribute.UIHint;
            }

            DataTypeAttribute dataTypeAttribute = attributeList.OfType<DataTypeAttribute>().FirstOrDefault();
            if (dataTypeAttribute != null)
            {
                result.DataTypeName = dataTypeAttribute.ToString();
            }

            EditableAttribute editable = attributes.OfType<EditableAttribute>().FirstOrDefault();
            if (editable != null)
            {
                result.IsReadOnly = !editable.AllowEdit;
            }
            else
            {
                ReadOnlyAttribute readOnlyAttribute = attributeList.OfType<ReadOnlyAttribute>().FirstOrDefault();
                if (readOnlyAttribute != null)
                {
                    result.IsReadOnly = readOnlyAttribute.IsReadOnly;
                }
            }

            DisplayFormatAttribute displayFormatAttribute = attributeList.OfType<DisplayFormatAttribute>().FirstOrDefault();
            if (displayFormatAttribute == null && dataTypeAttribute != null)
            {
                displayFormatAttribute = dataTypeAttribute.DisplayFormat;
            }
            if (displayFormatAttribute != null)
            {
                result.NullDisplayText = displayFormatAttribute.NullDisplayText;
                result.DisplayFormatString = displayFormatAttribute.DataFormatString;
                result.ConvertEmptyStringToNull = displayFormatAttribute.ConvertEmptyStringToNull;

                if (displayFormatAttribute.ApplyFormatInEditMode)
                {
                    result.EditFormatString = displayFormatAttribute.DataFormatString;
                }

                if (!displayFormatAttribute.HtmlEncode && String.IsNullOrWhiteSpace(result.DataTypeName))
                {
                    result.DataTypeName = DataType.Html.ToString();
                }
            }

            ScaffoldColumnAttribute scaffoldColumnAttribute = attributeList.OfType<ScaffoldColumnAttribute>().FirstOrDefault();
            if (scaffoldColumnAttribute != null)
            {
                result.ShowForDisplay = result.ShowForEdit = scaffoldColumnAttribute.Scaffold;
            }

            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            string name = null;
            if (display != null)
            {
                result.Description = display.GetDescription();
                result.ShortDisplayName = display.GetShortName();
                result.Watermark = display.GetPrompt();
                result.Order = display.GetOrder() ?? ModelMetadata.DefaultOrder;

                name = display.GetName();
            }

            if (name != null)
            {
                result.DisplayName = name;
            }
            else
            {
                DisplayNameAttribute displayNameAttribute = attributeList.OfType<DisplayNameAttribute>().FirstOrDefault();
                if (displayNameAttribute != null)
                {
                    result.DisplayName = displayNameAttribute.DisplayName;
                }
            }

            RequiredAttribute requiredAttribute = attributeList.OfType<RequiredAttribute>().FirstOrDefault();
            if (requiredAttribute != null)
            {
                result.IsRequired = true;
            }
        }
    }
    public class NoodleViewResult : ViewResult
    {

    }
    public class NoodlePartialViewResult : PartialViewResult
    {

    }

}