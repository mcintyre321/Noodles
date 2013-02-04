using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Noodles.WebApi.Models;
using Walkies;

namespace Noodles.WebApi
{

    public class NoodlesHttpMessageHandler : HttpMessageHandler
    {
        static NoodlesHttpMessageHandler()
        {
            Configuration.Initialise();
        }
        public ICollection<NoodlesHttpProcessor> Processors = new List<NoodlesHttpProcessor>();
        IEnumerable<NoodlesHttpProcessor> DefaultProcessors()
        {
            yield return InvokeNodeMethod;
            yield return ReturnObject;

        }

        private readonly Func<HttpRequestMessage, object> _getRootObject;
        private Func<NodeMethod, object[], object> _doInvoke;

        public NoodlesHttpMessageHandler(Func<HttpRequestMessage, object> getRootObject, Func<NodeMethod, object[], object> doInvoke = null)
        {
            _getRootObject = getRootObject;
            _doInvoke = doInvoke ?? ((nm, parameters) => nm.Invoke(parameters));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var root = _getRootObject(request);
            var routeData = request.Properties["MS_HttpRouteData"] as HttpRouteData;
            var path = routeData.Values["path"] as string ?? "/";
            object target = null;
            try
            {
                target = root.Walk(path.Trim('/')).Last();
            }
            catch (NodeNotFoundException ex)
            {
                return (request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
            if (target == null)
                return (request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found"));


            foreach (var processor in Processors.Concat(DefaultProcessors()))
            {
                var response = await processor(request, cancellationToken, target);
                if (response != null) return response;
            }

            return (request.CreateErrorResponse(HttpStatusCode.Conflict, "Was not a valid Noodles request"));
        }

        Task<T> MakeTask<T>(T t)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(t);
            return tcs.Task;
        }

        private async Task<HttpResponseMessage> InvokeNodeMethod(HttpRequestMessage request, CancellationToken cancellationToken, object target)
        {
            var nodeMethod = target as NodeMethod;
            if (nodeMethod == null) return null;

            var httpMethod = request.Method;
            var isInvoke = httpMethod == HttpMethod.Post || (httpMethod == HttpMethod.Get && nodeMethod.GetAttribute<HttpGetAttribute>() != null);

            if (!isInvoke) return null;

            var parameters = await new PostParameterBinder()
                .BindParameters(nodeMethod, request, cancellationToken);
            object result = null;
            try
            {
                result = _doInvoke(nodeMethod, parameters);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ex = ex.InnerException ?? ex;
                }
                throw;
            }
            var response = request.CreateResponse(HttpStatusCode.RedirectMethod);
            response.Headers.Location = new Uri(nodeMethod.Parent().Url(), UriKind.RelativeOrAbsolute);
            return response;
        }
        private Task<HttpResponseMessage> ReturnObject(HttpRequestMessage request, CancellationToken token, object target)
        {

            return MakeTask(request.CreateResponse(HttpStatusCode.OK, new ResourceVm(target)));
        }
        public static T BindObject<T>(HttpRequestMessage request, string name) where T : class
        {
            return null;// BindObject(cc, typeof(T), name, null, null) as T;
        }
    }
}