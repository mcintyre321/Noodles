using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;

namespace Noodles.WebApi
{
    class Error
    {
        public static Exception ArgumentNull(string type)
        {
            return new ArgumentNullException();
        }

        public static Exception Argument(string key, object jQuerySyntaxMissingClosingBracket)
        {
            return new ArgumentNullException();
            

        }

        public static Exception InvalidOperation(object maxHttpCollectionKeyLimitReached, int maxHttpCollectionKeys, Type type)
        {
            return new Exception();
        }
    }
    public static class FormDataCollectionExtensions
    {
        // This is a helper method to use Model Binding over a JQuery syntax. 
        // Normalize from JQuery to MVC keys. The model binding infrastructure uses MVC keys
        // x[] --> x
        // [] --> ""
        // x[field]  --> x.field, where field is not a number
        internal static string NormalizeJQueryToMvc(string key)
        {
            if (key == null)
            {
                return String.Empty;
            }

            StringBuilder sb = null;
            int i = 0;
            while (true)
            {
                int indexOpen = key.IndexOf('[', i);
                if (indexOpen < 0)
                {
                    // Fast path, no normalization needed.
                    // This skips the string conversion and allocating the string builder.
                    if (i == 0)
                    {
                        return key;
                    }
                    sb = sb ?? new StringBuilder();
                    sb.Append(key, i, key.Length - i);
                    break; // no more brackets
                }

                sb = sb ?? new StringBuilder();
                sb.Append(key, i, indexOpen - i); // everything up to "["

                // Find closing bracket.
                int indexClose = key.IndexOf(']', indexOpen);
                if (indexClose == -1)
                {
                    throw Error.Argument("key", "SRResources.JQuerySyntaxMissingClosingBracket");
                }

                if (indexClose == indexOpen + 1)
                {
                    // Empty bracket. Signifies array. Just remove. 
                }
                else
                {
                    if (Char.IsDigit(key[indexOpen + 1]))
                    {
                        // array index. Leave unchanged. 
                        sb.Append(key, indexOpen, indexClose - indexOpen + 1);
                    }
                    else
                    {
                        // Field name.  Convert to dot notation. 
                        sb.Append('.');
                        sb.Append(key, indexOpen + 1, indexClose - indexOpen - 1);
                    }
                }

                i = indexClose + 1;
                if (i >= key.Length)
                {
                    break; // end of string
                }
            }
            return sb.ToString();
        }

        internal static IEnumerable<KeyValuePair<string, string>> GetJQueryNameValuePairs(this FormDataCollection formData)
        {
            if (formData == null)
            {
                throw Error.ArgumentNull("formData");
            }

            int count = 0;

            foreach (KeyValuePair<string, string> kv in formData)
            {
                ThrowIfMaxHttpCollectionKeysExceeded(count);

                string key = NormalizeJQueryToMvc(kv.Key);
                string value = kv.Value ?? String.Empty;
                yield return new KeyValuePair<string, string>(key, value);

                count++;
            }
        }

        private static void ThrowIfMaxHttpCollectionKeysExceeded(int count)
        {
            if (count >= MediaTypeFormatter.MaxHttpCollectionKeys)
            {
                throw Error.InvalidOperation("SRResources.MaxHttpCollectionKeyLimitReached", MediaTypeFormatter.MaxHttpCollectionKeys, typeof(MediaTypeFormatter));
            }
        }

        // Create a IValueProvider for the given form, assuming a JQuery syntax.
        internal static IValueProvider GetJQueryValueProvider(this FormDataCollection formData)
        {
            if (formData == null)
            {
                throw Error.ArgumentNull("formData");
            }

            IEnumerable<KeyValuePair<string, string>> nvc = formData.GetJQueryNameValuePairs();
            return new NameValuePairsValueProvider(nvc, CultureInfo.InvariantCulture);
        }

        public static T ReadAs<T>(this FormDataCollection formData)
        {
            return (T)ReadAs(formData, typeof(T));
        }

        public static object ReadAs(this FormDataCollection formData, Type type)
        {
            return ReadAs(formData, type, String.Empty, requiredMemberSelector: null, formatterLogger: null);
        }

        public static T ReadAs<T>(this FormDataCollection formData, string modelName, IRequiredMemberSelector requiredMemberSelector, IFormatterLogger formatterLogger)
        {
            return (T)ReadAs(formData, typeof(T), modelName, requiredMemberSelector, formatterLogger);
        }

        /// <summary>
        /// Deserialize the form data to the given type, using model binding.  
        /// </summary>
        /// <param name="formData">collection with parsed form url data</param>
        /// <param name="type">target type to read as</param>
        /// <param name="modelName">null or empty to read the entire form as a single object. This is common for body data. 
        /// <param name="requiredMemberSelector">The <see cref="IRequiredMemberSelector"/> used to determine required members.</param>
        /// <param name="formatterLogger">The <see cref="IFormatterLogger"/> to log events to.</param>
        /// Or the name of a model to do a partial binding against the form data. This is common for extracting individual fields.</param>
        /// <returns>best attempt to bind the object. The best attempt may be null.</returns>
        public static object ReadAs(this FormDataCollection formData, Type type, string modelName, IRequiredMemberSelector requiredMemberSelector, IFormatterLogger formatterLogger)
        {
            if (formData == null)
            {
                throw Error.ArgumentNull("formData");
            }
            if (type == null)
            {
                throw Error.ArgumentNull("type");
            }

            if (modelName == null)
            {
                modelName = String.Empty;
            }

            using (HttpConfiguration config = new HttpConfiguration())
            {
                bool validateRequiredMembers = requiredMemberSelector != null && formatterLogger != null;
                if (validateRequiredMembers)
                {
                    // Set a ModelValidatorProvider that understands the IRequiredMemberSelector
                    config.Services.Replace(typeof(ModelValidatorProvider), new RequiredMemberModelValidatorProvider(requiredMemberSelector));
                }

                // Looks like HttpActionContext is just a way of getting to the config, which we really
                // just need to get a list of modelbinderPRoviders for composition. 
                HttpActionContext actionContext = CreateActionContextForModelBinding(config);

                IValueProvider vp = formData.GetJQueryValueProvider();
                ModelBindingContext ctx = CreateModelBindingContext(actionContext, modelName, type, vp);

                ModelBinderProvider modelBinderProvider = CreateModelBindingProvider(actionContext);

                IModelBinder binder = modelBinderProvider.GetBinder(config, type);
                bool haveResult = binder.BindModel(actionContext, ctx);

                // Log model binding errors
                if (formatterLogger != null)
                {
                    foreach (KeyValuePair<string, ModelState> modelStatePair in actionContext.ModelState)
                    {
                        foreach (ModelError modelError in modelStatePair.Value.Errors)
                        {
                            if (modelError.Exception != null)
                            {
                                formatterLogger.LogError(modelStatePair.Key, modelError.Exception);
                            }
                            else
                            {
                                formatterLogger.LogError(modelStatePair.Key, modelError.ErrorMessage);
                            }
                        }
                    }
                }

                if (haveResult)
                {
                    return ctx.Model;
                }
                return MediaTypeFormatter.GetDefaultValueForType(type);
            }
        }

        // Helper for ReadAs() to get a ModelBinderProvider to read FormUrl data. 
        private static ModelBinderProvider CreateModelBindingProvider(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            ServicesContainer cs = actionContext.ControllerContext.Configuration.Services;
            IEnumerable<ModelBinderProvider> providers = cs.GetModelBinderProviders();
            ModelBinderProvider modelBinderProvider = new CompositeModelBinderProvider(providers);
            return modelBinderProvider;
        }

        // Helper for ReadAs() to get a ModelBindingContext to invoke model binding over FormUrl data. 
        private static ModelBindingContext CreateModelBindingContext(HttpActionContext actionContext, string modelName, Type type, IValueProvider vp)
        {
            Contract.Assert(actionContext != null);
            Contract.Assert(type != null);
            Contract.Assert(vp != null);

            ServicesContainer cs = actionContext.ControllerContext.Configuration.Services;
            ModelMetadataProvider metadataProvider = cs.GetModelMetadataProvider();

            ModelBindingContext ctx = new ModelBindingContext()
            {
                ModelName = modelName,
                FallbackToEmptyPrefix = false,
                ModelMetadata = metadataProvider.GetMetadataForType(null, type),
                ModelState = actionContext.ModelState,
                ValueProvider = vp
            };
            return ctx;
        }

        // Creates a default action context to invoke model binding
        private static HttpActionContext CreateActionContextForModelBinding(HttpConfiguration config)
        {
            Contract.Assert(config != null);

            HttpControllerContext controllerContext = new HttpControllerContext() { Configuration = config };
            controllerContext.ControllerDescriptor = new HttpControllerDescriptor();

            HttpActionContext actionContext = new HttpActionContext { ControllerContext = controllerContext };

            return actionContext;
        }
    }
    public class JQueryMvcFormUrlEncodedFormatter : FormUrlEncodedMediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (readStream == null)
            {
                throw new ArgumentNullException("readStream");
            }

            // For simple types, defer to base class
            if (base.CanReadType(type))
            {
                return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
            }

            return ReadFromStreamAsyncCore(type, readStream, content, formatterLogger);
        }

        private async Task<object> ReadFromStreamAsyncCore(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            object obj = await base.ReadFromStreamAsync(typeof(FormDataCollection), readStream, content, formatterLogger);
            FormDataCollection fd = (FormDataCollection)obj;

            try
            {
                return fd.ReadAs(type, String.Empty, RequiredMemberSelector, formatterLogger);
            }
            catch (Exception e)
            {
                if (formatterLogger == null)
                {
                    throw;
                }
                formatterLogger.LogError(String.Empty, e);
                return GetDefaultValueForType(type);
            }
        }
    }
    public sealed class CompositeModelBinderProvider : ModelBinderProvider
    {
        private ModelBinderProvider[] _providers;

        public CompositeModelBinderProvider()
        {
        }

        public CompositeModelBinderProvider(IEnumerable<ModelBinderProvider> providers)
        {
            if (providers == null)
            {
                throw Error.ArgumentNull("providers");
            }

            _providers = providers.ToArray();
        }

        public IEnumerable<ModelBinderProvider> Providers
        {
            get { return _providers; }
        }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            IEnumerable<ModelBinderProvider> providers = _providers ?? configuration.Services.GetModelBinderProviders();

            // Pre-filter out any binders that we know can't match. 
            IEnumerable<IModelBinder> binders = from provider in providers
                                                let binder = provider.GetBinder(configuration, modelType)
                                                where binder != null
                                                select binder;
            return new CompositeModelBinder(binders);
        }
    }
    public class CompositeModelBinder : IModelBinder
    {
        public CompositeModelBinder(IEnumerable<IModelBinder> binders)
            : this(binders.ToArray())
        {
        }

        public CompositeModelBinder(params IModelBinder[] binders)
        {
            Binders = binders;
        }

        private IModelBinder[] Binders { get; set; }

        public virtual bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            ModelBindingContext newBindingContext = CreateNewBindingContext(bindingContext, bindingContext.ModelName);

            bool boundSuccessfully = TryBind(actionContext, newBindingContext);
            if (!boundSuccessfully && !String.IsNullOrEmpty(bindingContext.ModelName)
                && bindingContext.FallbackToEmptyPrefix)
            {
                // fallback to empty prefix?
                newBindingContext = CreateNewBindingContext(bindingContext, modelName: String.Empty);
                boundSuccessfully = TryBind(actionContext, newBindingContext);
            }

            if (!boundSuccessfully)
            {
                return false; // something went wrong
            }

            // run validation and return the model
            // If we fell back to an empty prefix above and are dealing with simple types,
            // propagate the non-blank model name through for user clarity in validation errors.
            // Complex types will reveal their individual properties as model names and do not require this.
            if (!newBindingContext.ModelMetadata.IsComplexType && String.IsNullOrEmpty(newBindingContext.ModelName))
            {
                //newBindingContext.ValidationNode = new Validation.ModelValidationNode(newBindingContext.ModelMetadata, bindingContext.ModelName);
            }

            newBindingContext.ValidationNode.Validate(actionContext, null /* parentNode */);
            bindingContext.Model = newBindingContext.Model;
            return true;
        }

        private bool TryBind(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            return actionContext.Bind(bindingContext, Binders);
        }

        private static ModelBindingContext CreateNewBindingContext(ModelBindingContext oldBindingContext, string modelName)
        {
            ModelBindingContext newBindingContext = new ModelBindingContext
            {
                ModelMetadata = oldBindingContext.ModelMetadata,
                ModelName = modelName,
                ModelState = oldBindingContext.ModelState,
                ValueProvider = oldBindingContext.ValueProvider
            };

            // validation is expensive to create, so copy it over if we can
            if (Object.ReferenceEquals(modelName, oldBindingContext.ModelName))
            {
                newBindingContext.ValidationNode = oldBindingContext.ValidationNode;
            }

            return newBindingContext;
        }
    }
}
