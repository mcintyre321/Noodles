using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Noodles.Models;
using Noodles.RequestHandling;

namespace Noodles.AspMvc.RequestHandling
{
    class AspMvcRequestInfo : RequestInfo
    {
        private readonly ControllerContext _cc;
        private Uri _rootUrl;

        public AspMvcRequestInfo(ControllerContext cc)
        {
            _cc = cc;
        }

        public override bool IsInvoke(IInvokeable method)
        {
            if (_cc.HttpContext.Request.HttpMethod == "POST") return true;
            if (_cc.HttpContext.Request.HttpMethod == "GET" && method.GetAttribute<HttpGetAttribute>() != null)
            {
                return true;
            }
            return false;
        }

        public override Uri RootUrl
        {
            get
            {
                return _rootUrl ?? (_rootUrl = new Uri(new UrlHelper(_cc.RequestContext).Action(_cc.RequestContext.RouteData.Values["action"] as string,
                                                  _cc.RequestContext.RouteData.Values["controller"] as string,
                                                  new { path = "" }), UriKind.Relative));
            }
        }

        public override async Task<ArgumentBinding[]> GetArgumentBindings(IInvokeable method)
        {
            var invokeableParameters = method.Parameters;
            var boundValues = new List<ArgumentBinding>();
            foreach (var invokeableParameter in invokeableParameters)
            {
                var parameterObject = BindObject(_cc, invokeableParameter,  method.InvokeDisplayName);
                boundValues.Add(parameterObject);
            }

            return await Task.FromResult(boundValues.ToArray());
        }

        private static ArgumentBinding BindObject(ControllerContext cc, IInvokeableParameter parameter,  string displayName)
        {
            var attributes =  parameter.Attributes.ToArray();
            displayName = displayName ?? parameter.Name.Sentencise(true);

            cc.HttpContext.Request.InputStream.Position = 0; //reset as Json binder will have already read it once

            var valueProvider = ValueProviderFactories.Factories.GetValueProvider(cc);

            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, parameter.ValueType);
            metadata.DisplayName = displayName;
            ApplyAttributeMetaData(metadata, attributes);
            var bindingContext = new ModelBindingContext
            {
                ModelName = parameter.Name,
                ValueProvider = valueProvider,
                ModelMetadata = metadata,
                ModelState = cc.Controller.ViewData.ModelState
            };

            var binder = ModelBinders.Binders.GetBinder(parameter.ValueType, true);
            var output = binder.BindModel(cc, bindingContext);

            var errors = new List<string>();

            foreach (var va in attributes.OfType<ValidationAttribute>())
            {
                var result = va.GetValidationResult(output,
                    new ValidationContext(parameter.Parent, null, null) {MemberName = parameter.Name});
                if (result != ValidationResult.Success)
                {
                    var formatErrorMessage = va.FormatErrorMessage(parameter.DisplayName);
                    errors.Add(formatErrorMessage);
                    //bindingContext.ModelState.AddModelError(parameter.Name, formatErrorMessage);
                }
            }

            var argumentBinding = new ArgumentBinding()
            {
                Parameter = parameter,
                Value = output,
                Errors = errors
            };
            return argumentBinding;
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
                System.ComponentModel.DisplayNameAttribute displayNameAttribute = attributeList.OfType<System.ComponentModel.DisplayNameAttribute>().FirstOrDefault();
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

            var validateInputAttributes = attributes.OfType<ValidateInputAttribute>().SingleOrDefault();
            if (validateInputAttributes != null && validateInputAttributes.EnableValidation == false)
                result.RequestValidationEnabled = false;
        }
    }
}