using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Noodles.Example.WebApi
{
    public class PostParameterBinder
    {

        public async Task<object[]> BindParameters(NodeMethod nm, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var config = (HttpConfiguration) request.Properties["MS_HttpConfiguration"];
            var formatter = config.Formatters.FindReader(nm.SignatureType, request.Content.Headers.ContentType);
            var stream = await request.Content.ReadAsStreamAsync();
            var modelState = new ModelStateDictionary();
            var logger = new ModelStateFormatterLogger(modelState, "model");
            var obj = await formatter.ReadFromStreamAsync(nm.SignatureType, stream, request.Content, logger);
            return obj.GetType().GetProperties().Select(pi => pi.GetValue(obj, null)).ToArray();
        }

    }
}