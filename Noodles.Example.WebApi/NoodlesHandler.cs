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
using Noodles.Example.WebApi.Models;
using Walkies;

namespace Noodles.Example.WebApi
{

    public class NoodlesHandler : HttpMessageHandler
    {
        static NoodlesHandler() 
        {
            //ModelStateExceptionHandlers.Add((e, msd) => (e as TEx) == null ? null as Action : () => action((TEx)e, msd));
            //ModelStateExceptionHandlers.Add((e, msd) => (e as NodeNotFoundException) == null ? null as Action : () => action((TEx)e, msd));

            Walkies.WalkExtension.Rules.Add((o, fragment) => fragment == "actions" ? new NodeMethods(o) : null);
        }
        public ICollection<NoodlesHttpProcessor> Processors = new List<NoodlesHttpProcessor>();
        IEnumerable<NoodlesHttpProcessor> DefaultProcessors()
        {
            yield return InvokeNodeMethod;
            yield return ReturnObject;

        }

        private readonly Func<HttpRequestMessage, object> _getRootObject;
        private Func<NodeMethod, object[], object> _doInvoke;

        public NoodlesHandler(Func<HttpRequestMessage, object> getRootObject, Func<NodeMethod, object[], object> doInvoke = null)
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

            var routeData = request.Properties["MS_HttpRouteData"] as HttpRouteData;
           
            var config = (HttpConfiguration)request.Properties["MS_HttpConfiguration"];
            var nodeMethodHttpControllerDescriptor = new NodeMethodHttpControllerDescriptor();
            var nodeMethodHttpActionDescriptor = new NodeMethodHttpActionDescriptor(nodeMethod, config)
            {
                ControllerDescriptor = nodeMethodHttpControllerDescriptor
            };
            var parameterBindings = nodeMethodHttpActionDescriptor.GetParameters();
            var parameters = await new JsonPostVariableParameterBinder(parameterBindings).BindParameters(nodeMethod, request, cancellationToken);
            object result = null;
            try
            {
                result = _doInvoke(nodeMethod, parameters.ToArray());
                //if (result is ActionResult) return (ActionResult)result;
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ex = ex.InnerException ?? ex;
                }
                //Action handle = ModelStateExceptionHandlers.Select(h => h(ex, cc)).FirstOrDefault(h => h != null);

                //if (handle != null)
                //{
                //    //cc.HttpContext.Response.StatusCode = 409;
                //    handle();
                //}
                //else
                //{
                    //cc.HttpContext.Response.StatusCode = 500;
                    throw;
                //}
            }
            //var msd = cc.Controller.ViewData.ModelState;

            //object result = null;
            
            //else
            //{
            //    cc.HttpContext.Response.StatusCode = 409;
            //}

            //cc.HttpContext.Response.TrySkipIisCustomErrors = true;

            //var nodeMethodReturnUrl = cc.RequestContext.HttpContext.Request["nodeMethodReturnUrl"];
            //if (!cc.HttpContext.Request.IsAjaxRequest() && msd.IsValid)
            //{
            //    return new RedirectResult(nodeMethodReturnUrl);
            //}
            var response = request.CreateResponse(HttpStatusCode.OK, new NodeMethodInvokeSuccess(nodeMethod));
            //ViewResultBase res = cc.HttpContext.Request.IsAjaxRequest() ? (ViewResultBase)new NoodlePartialViewResult() : new NoodleViewResult();
            //if (msd.IsValid)
            //{
            //    res.ViewName = "Noodles/NodeMethodSuccess";
            //    res.ViewData.Model = new NodeMethodSuccessVm(method, result);
            //}
            //else
            //{
            //    res.ViewName = "Noodles/Action";
            //    res.ViewData.Model = method;
            //}
            //res.ViewData.ModelState.Merge(msd);
            //res.ViewData["nodeMethodReturnUrl"] = nodeMethodReturnUrl;
            //return res;
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