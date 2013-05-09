using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Noodles.Models;
using Noodles.RequestHandling;
using Noodles.RequestHandling.ResultTypes;
using Noodles.WebApi.Models;

namespace Noodles.WebApi
{
    public class NoodlesToWebApiResultMapper : NoodleResultMapper<Task<HttpResponseMessage>, HttpRequestMessage>
    {
        #region Overrides of NoodleResultMapper<HttpResponseMessage,HttpRequestMessage>

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, BadRequestResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.BadRequest, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, ErrorResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.InternalServerError, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, NotFoundResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.NotFound, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, ValidationErrorResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.Conflict, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, ViewResult result)
        {
            var target = result.Target;
            var type = target.GetType();
            if (typeof(Resource).IsAssignableFrom(type))
            {
                return Task.FromResult(context.CreateResponse(HttpStatusCode.OK, new ResourceVm((Resource) result.Target)));
            }
            else if (type == typeof(NodeProperty))
            {
                return Task.FromResult(context.CreateResponse(HttpStatusCode.OK, new PropertyVm((NodeProperty)result.Target)));

            }
            else if (type == typeof(NodeCollectionProperty))
            {
                return Task.FromResult(context.CreateResponse(HttpStatusCode.OK, new CollectionVm((NodeCollectionProperty)result.Target)));

            }
            else if (type == typeof(NodeMethod))
            {
                return Task.FromResult(context.CreateResponse(HttpStatusCode.OK, new ActionVm((NodeMethod)result.Target)));

            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, InvokeSuccessResult result)
        {
            return Task.FromResult(context.CreateResponse(HttpStatusCode.OK, new InvokeVm(result.Invokeable)));
        }

        #endregion
    }
}