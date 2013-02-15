using System;
using Noodles.Requests.Results;

namespace Noodles.Requests
{
    public abstract class NoodleResultMapper<TOutputResult, TContext>
    {
        public TOutputResult Map(TContext context, NoodlesResult result)
        {
            if (result is BadRequestResult) return Map(context, (BadRequestResult)result);
            if (result is NoodlesErrorResult) return Map(context, (NoodlesErrorResult)result);
            if (result is NotFoundResult) return Map(context, (NotFoundResult)result);
            if (result is NoodlesValidationErrorResult) return Map(context, (NoodlesValidationErrorResult)result);
            if (result is NoodlesViewResult) return Map(context, (NoodlesViewResult)result);
            if (result is InvokeSuccessResult) return Map(context, (InvokeSuccessResult)result);
            throw new NotImplementedException();
        }

        public abstract TOutputResult Map(TContext context, BadRequestResult result);
        public abstract TOutputResult Map(TContext context, NoodlesErrorResult result);
        public abstract TOutputResult Map(TContext context, NotFoundResult result);
        public abstract TOutputResult Map(TContext context, NoodlesValidationErrorResult result);
        public abstract TOutputResult Map(TContext context, NoodlesViewResult result);
        public abstract TOutputResult Map(TContext context, InvokeSuccessResult result);
    }
}