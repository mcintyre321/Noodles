using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Noodles.Requests.Results;

namespace Noodles.Requests
{
    public class DefaultProcessors<TContext>
    {
        public static async Task<NoodlesResult> Read(TContext context1, NoodlesRequest request, INode node, Func<IInvokeable, object[], object> doinvoke)
        {
            var result = MapResultToNoodleResult(node);
            if (result != null) return await Task.Factory.StartNew(() => result);
            if (!request.IsInvoke)
            {
                return await Task.Factory.StartNew<NoodlesResult>(() => new NoodlesViewResult(node));
            }
            return await Task.Factory.StartNew<NoodlesResult>(() => null);
        }

        static Task<NoodlesResult> NullTask()
        {
            return Task.Factory.StartNew<NoodlesResult>(() => null);
        }


        public static async Task<NoodlesResult> ProcessInvoke(TContext context, NoodlesRequest request, INode node, Func<IInvokeable, object[], object> doInvoke)
        {
            var invokeable = node as IInvokeable;
            if (invokeable == null) return await NullTask();

            var isInvoke = request.IsInvoke;
            if (!isInvoke) return null;

            IEnumerable<object> parameters = null;
            try
            {
                parameters = await request.GetArguments(invokeable);
            }
            catch (ArgumentBindingException ex)
            {

            }
            object result = null;
            var errors = new List<object>();
            if (parameters != null)
            {
                Logger.Trace("ModelBinding successful");
                try
                {
                    result = doInvoke(invokeable, parameters.ToArray());
                    var noodlesResult = MapResultToNoodleResult(result);
                    if (noodlesResult != null) return noodlesResult;
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException)
                    {
                        ex = ex.InnerException ?? ex;
                    }
                    Func<NoodlesResult> handle = null;
                    // ModelStateExceptionHandlers.Select(h => h(ex, cc)).FirstOrDefault(h => h != null);

                    if (handle != null) //there is a handler for this exception
                    {
                        var handlerResult = handle(); //try to handle it
                        if (handlerResult != null) return handlerResult; //return the handler result if it had one
                    }
                    else
                    {
                        return new NoodlesErrorResult();
                    }
                }
            }
            else
            {
                return new NoodlesValidationErrorResult(invokeable);
            }

            {
                return new InvokeSuccessResult(invokeable);
            }
        }

        private static NoodlesResult MapResultToNoodleResult(object result)
        {
            if (result is INode) return new NoodlesViewResult((INode) result);
            return null;
        }


    }
}