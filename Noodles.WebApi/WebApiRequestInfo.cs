using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Noodles.Models;
using Noodles.RequestHandling;

namespace Noodles.WebApi
{
    public class WebApiRequestInfo : RequestInfo
    {
        private Uri _rootUrl;
        private HttpRequestMessage _request;
        private CancellationToken _ct;

        public WebApiRequestInfo(HttpRequestMessage request, CancellationToken ct)
        {
            _request = request;
            _ct = ct;
            var routeData = request.Properties["MS_HttpRouteData"] as IHttpRouteData;
            
            _rootUrl = new Uri('/' + routeData.Route.RouteTemplate.Substring(0, routeData.Route.RouteTemplate.IndexOf("{*path}")).Trim('/') + '/', UriKind.Relative);
        }

        public override bool IsInvoke(IInvokeable invokeable)
        {
            return _request.Method == HttpMethod.Post;
        }

        public override Uri RootUrl
        {
            get { return _rootUrl; }
        }

        public override async Task<IEnumerable<Tuple<string, object>>> GetArguments(IInvokeable method)
        {
            var binder = new PostParameterBinder();
            return await binder.BindParameters(method, _request, _ct);
        }
    }
}