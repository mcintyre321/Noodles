using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Noodles.Models;
using Noodles.RequestHandling;
using Walkies;

namespace Noodles.WebApi
{

    public class NoodlesHttpMessageHandler : HttpMessageHandler
    {
       
        private readonly Func<HttpRequestMessage, object> _getRootObject;
        private Func<IInvokeable, object[], object> _doInvoke;

        public NoodlesHttpMessageHandler(Func<HttpRequestMessage, object> getRootObject, Func<IInvokeable, object[], object> doInvoke = null)
        {
            _getRootObject = getRootObject;
            _doInvoke = doInvoke ?? ((nm, parameters) => nm.Invoke(parameters));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var routeData = request.Properties["MS_HttpRouteData"] as IHttpRouteData;
            var path = routeData.Values["path"] as string ?? "/";
            var root = _getRootObject(request);
            root.SetUrlRoot(routeData.Route.RouteTemplate.Substring(0, routeData.Route.RouteTemplate.IndexOf("{*path}")));
            var handler = new WebApiNoodlesHandler();
            var webApiNoodlesRequest = new WebApiRequestInfo(request, cancellationToken);
            var result = await handler.HandleRequest(request, webApiNoodlesRequest, root, path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries), _doInvoke);
            var mapper = new NoodlesToWebApiResultMapper();
            return await mapper.Map(request, result);
        }
 
 
    }

    public class WebApiNoodlesHandler : Handler<HttpRequestMessage>
    {
    }
}