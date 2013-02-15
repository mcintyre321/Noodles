using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Noodles.Requests;

namespace Noodles.AspMvc.Requests
{
    class AspMvcNoodlesRequest : NoodlesRequest
    {
        private readonly ControllerContext _cc;
        private string _rootUrl;

        public AspMvcNoodlesRequest(ControllerContext cc)
        {
            _cc = cc;
        }

        public override string RootUrl
        {
            get
            {
                return _rootUrl ?? (_rootUrl = new UrlHelper(_cc.RequestContext)
                                          .Action(_cc.RequestContext.RouteData.Values["action"] as string,
                                                  _cc.RequestContext.RouteData.Values["controller"] as string,
                                                  new { path = "" }));
            }
        }

        public override async Task<IEnumerable<object>> GetArguments(IInvokeable method)
        {
            var parameters = method.Parameters.Select(pt => BindObject(_cc, pt.ParameterType, pt.Name, pt.CustomAttributes, pt.DisplayName)).ToArray();
            return await Task.FromResult<IEnumerable<Object>>(parameters);
        }

        private static object BindObject(ControllerContext cc, Type desiredType, string name, IEnumerable<Attribute> attributes, string displayName)
        {
            attributes = attributes ?? new Attribute[] { };
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
        }
    }
}