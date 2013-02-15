using System;
using Noodles.Requests.Results;

namespace Noodles.Requests
{
    public abstract class NoodleResultMapper<TOutputResult, TContext>
    {
        public TOutputResult Map(TContext context, Result result)
        {
            if (result is BadRequestResult) return Map(context, (BadRequestResult)result);
            if (result is ErrorResult) return Map(context, (ErrorResult)result);
            if (result is NotFoundResult) return Map(context, (NotFoundResult)result);
            if (result is ValidationErrorResult) return Map(context, (ValidationErrorResult)result);
            if (result is ViewResult) return Map(context, (ViewResult)result);
            if (result is InvokeSuccessResult) return Map(context, (InvokeSuccessResult)result);
            throw new NotImplementedException();
        }

        public abstract TOutputResult Map(TContext context, BadRequestResult result);
        public abstract TOutputResult Map(TContext context, ErrorResult result);
        public abstract TOutputResult Map(TContext context, NotFoundResult result);
        public abstract TOutputResult Map(TContext context, ValidationErrorResult result);
        public abstract TOutputResult Map(TContext context, ViewResult result);
        public abstract TOutputResult Map(TContext context, InvokeSuccessResult result);
    }
}