using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Noodles.Requests;
using Noodles.Requests.Results;

namespace Noodles.WebApi
{
    public class NoodlesToWebApiResultMapper: Noodles.Requests.NoodleResultMapper<Task<HttpResponseMessage>, HttpRequestMessage>
    {
        #region Overrides of NoodleResultMapper<HttpResponseMessage,HttpRequestMessage>

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, BadRequestResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.BadRequest, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, NoodlesErrorResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.InternalServerError, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, NotFoundResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.NotFound, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, NoodlesValidationErrorResult result)
        {
            return Task.FromResult(context.CreateErrorResponse(HttpStatusCode.Conflict, ""));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, NoodlesViewResult result)
        {
            return Task.FromResult(context.CreateResponse(HttpStatusCode.OK, result.Node));
        }

        public override Task<HttpResponseMessage> Map(HttpRequestMessage context, InvokeSuccessResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}