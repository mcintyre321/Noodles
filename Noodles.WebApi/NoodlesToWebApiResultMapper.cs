using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Noodles.RequestHandling;
using Noodles.RequestHandling.ResultTypes;
using Noodles.WebApi.Models;

namespace Noodles.WebApi
{
    public class NoodlesToWebApiResultMapper: NoodleResultMapper<Task<HttpResponseMessage>, HttpRequestMessage>
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
            return Task.FromResult(context.CreateResponse(HttpStatusCode.OK, new ResourceVm(result.Node)));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, InvokeSuccessResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}