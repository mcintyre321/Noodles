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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Task = Noodles.Example.Domain.Task;

namespace Noodles.Example.WebApi
{
    public class JsonPostVariableParameterBinder
    {

        public JsonPostVariableParameterBinder(Collection<HttpParameterDescriptor> descriptor)
        {
        }

        public async Task<IEnumerable<object>> BindParameters(NodeMethod nm, HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var config = (HttpConfiguration) request.Properties["MS_HttpConfiguration"];
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().Single();

            JToken jToken = null;

            var body = await TryReadBody(request);
            var parameters = new List<object>();
            foreach (var param in nm.Parameters)
            {
                if (body != null)
                    jToken = body[param.Name];

                // try reading query string if we have no POST/PUT match
                if (jToken == null)
                {
                    var query = request.GetQueryNameValuePairs();
                    if (query != null)
                    {
                        var matches = query.Where(kv => kv.Key.ToLower() == param.Name.ToLower());
                        if (matches.Any())
                            jToken = JToken.Parse(matches.First().Value);
                    }
                }

                var serializer = JsonSerializer.Create(jsonFormatter.SerializerSettings);
                parameters.Add(jToken.ToObject(param.ParameterType, serializer));
            }
            return parameters;
        }


        /// <summary>
        /// Method that implements parameter binding hookup to the global configuration object's
        /// ParameterBindingRules collection delegate.
        /// 
        /// This routine filters based on POST/PUT method status and simple parameter
        /// types.
        /// </summary>
       


        /// <summary>
        /// Read and cache the request body
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<JObject> TryReadBody(HttpRequestMessage request)
        {
            object result = null;

            // try to read out of cache first
            if (!request.Properties.TryGetValue("JsonBodyParameters", out result))
            {
                var contentType = request.Content.Headers.ContentType;

                // only read if there's content and it's form data
                if (contentType == null || contentType.MediaType != "application/json")
                {
                    // Nope no data
                    result = null;
                }
                else
                {
                    var json = await request.Content.ReadAsStringAsync();
                    return JObject.Parse(json);
                }
            }
            return new JObject();
        }

        private struct AsyncVoid
        {
        }
    }
}