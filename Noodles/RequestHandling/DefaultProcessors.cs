using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Noodles.Models;
using Noodles.RequestHandling.ResultTypes;

namespace Noodles.RequestHandling
{
    public class DefaultProcessors<TContext>
    {
        public static async Task<Result> Read(TContext context1, RequestInfo requestInfo, INode node, Func<IInvokeable, IDictionary<string, object>, object> doinvoke)
        {
            var result = MapResultToNoodleResult(node);
            if (result != null) return await Task.Factory.StartNew(() => result);
            if (!requestInfo.IsInvoke)
            {
                return await Task.Factory.StartNew<Result>(() => new ViewResult(node));
            }
            return await Task.Factory.StartNew<Result>(() => null);
        }

        static Task<Result> NullTask()
        {
            return Task.Factory.StartNew<Result>(() => null);
        }


        public static async Task<Result> ProcessInvoke(TContext context1, RequestInfo requestInfo, INode node, Func<IInvokeable, IDictionary<string, object>, object> doInvoke)
        {
            doInvoke = doInvoke ?? DoInvoke;
            var invokeable = node as IInvokeable;
            if (invokeable == null) return await NullTask();

            var isInvoke = requestInfo.IsInvoke;
            if (!isInvoke) return null;

            IDictionary<string, object> parameters = null;
            try
            {
                var tuples = await requestInfo.GetArguments(invokeable);
                parameters = tuples.ToDictionary(t => t.Item1, t => t.Item2);
            }
            catch (ArgumentBindingException ex)
            {
                return new ValidationErrorResult(invokeable);
            }
            object result = null;
            var errors = new List<object>();
            if (parameters != null)
            {
                Logger.Trace("ModelBinding successful");
                try
                {
                    result = doInvoke(invokeable, parameters);
                    var noodlesResult = MapResultToNoodleResult(result);
                    if (noodlesResult != null) return noodlesResult;
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException)
                    {
                        ex = ex.InnerException ?? ex;
                    }
                    Func<Result> handle = null;
                    // ModelStateExceptionHandlers.Select(h => h(ex, cc)).FirstOrDefault(h => h != null);

                    if (handle != null) //there is a handler for this exception
                    {
                        var handlerResult = handle(); //try to handle it
                        if (handlerResult != null) return handlerResult; //return the handler result if it had one
                    }
                    else
                    {
                        return new ErrorResult();
                    }
                }
            }
            else
            {
                return new ValidationErrorResult(invokeable);
            }

            {
                return new InvokeSuccessResult(invokeable)
                    {
                        Result = result
                    };
            }
        }

       
        private static object DoInvoke(IInvokeable invokeable, IDictionary<string, object> args)
        {
            return invokeable.Invoke(args);
        }

        private static Result MapResultToNoodleResult(object result)
        {
            if (result is INode) return new ViewResult((INode) result);
            return null;
        }


    }
}