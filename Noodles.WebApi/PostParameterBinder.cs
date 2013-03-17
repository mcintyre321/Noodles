using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;
using Noodles.Models;

namespace Noodles.WebApi
{
    public class PostParameterBinder
    {

        public async Task<Tuple<string, object>[]> BindParameters(IInvokeable nm, HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var config = (HttpConfiguration) request.Properties["MS_HttpConfiguration"];
            var formatter = config.Formatters.FindReader(nm.ParameterType, request.Content.Headers.ContentType);
            //formatter = new Noodles.WebApi.JQueryMvcFormUrlEncodedFormatter();
            var stream = await request.Content.ReadAsStreamAsync();
            var modelState = new ModelStateDictionary();
            var logger = new ModelStateFormatterLogger(modelState, "model");
            var obj = await formatter.ReadFromStreamAsync(nm.ParameterType, stream, request.Content, logger);
            return obj == null ? Enumerable.Empty<Tuple<string, object>>().ToArray() : obj.GetType().GetProperties().Select(pi => Tuple.Create(pi.Name, pi.GetValue(obj, null))).ToArray();
        }

    }
}