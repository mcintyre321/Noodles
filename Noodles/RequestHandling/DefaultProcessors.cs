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
            if (result != null) return result;
            if (node is IInvokeable && !requestInfo.IsInvoke((IInvokeable) node))
            {
                return new ViewResult(node);
            }
            return null;
        }

        private static Result MapResultToNoodleResult(INode node)
        {

            return new ViewResult(node);
        }

        static Task<Result> NullTask()
        {
            return Task.Factory.StartNew<Result>(() => null);
        }

        public delegate Func<Result> InvokeExceptionHandler(Exception ex, RequestInfo ri, TContext context, IInvokeable invokeable);

        public static readonly List<InvokeExceptionHandler> InvokeExceptionHandlers = new List<InvokeExceptionHandler>()
        {
            ReturnErrorOnUserException
        };

        private static Func<Result> ReturnErrorOnUserException(Exception ex, RequestInfo ri, TContext context, IInvokeable invokeable)
        {
            var uEx = ex as UserException;
            if (uEx == null) return null;
            return () => new ValidationErrorResult(invokeable)
            {
                {"", ex.Message}
            };
        }

        public static async Task<Result> ProcessInvoke(TContext context1, RequestInfo requestInfo, INode node,
                                                       Func<IInvokeable, IDictionary<string, object>, object> doInvoke)
        {
            doInvoke = doInvoke ?? DoInvoke;
            var invokeable = node as IInvokeable;
            if (invokeable == null) return await NullTask();

            var isInvoke = requestInfo.IsInvoke(invokeable);
            if (!isInvoke) return null;

            IDictionary<string, object> parameters = null;
            var argumentBindings = await requestInfo.GetArgumentBindings(invokeable);
            var errors = from binding in argumentBindings
                         let allErrors = binding.Errors.Concat(GetValidationErrorsForValue(binding))
                         from e in allErrors
                         select new KeyValuePair<string, string>(binding.Parameter.Name, e);
            var errorsArray = errors as KeyValuePair<string, string>[] ?? errors.ToArray();
            if (errorsArray.Any())
            {
                return new ValidationErrorResult(invokeable){ errorsArray };
            }
            parameters = argumentBindings.ToDictionary(t => t.Parameter.Name, t => t.Value);
            object result = null;

            Logger.Trace("ModelBinding successful");
            try
            {
                result = doInvoke(invokeable, parameters);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ex = ex.InnerException ?? ex;
                }
                Func<Result> handle = InvokeExceptionHandlers.Select(h => h(ex, requestInfo, context1, invokeable))
                                                             .FirstOrDefault(h => h != null);

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

            return new InvokeSuccessResult(invokeable)
            {
                Result = result
            };
        }

        private static IEnumerable<string> GetValidationErrorsForValue(ArgumentBinding binding)
        {
            if (binding.Parameter.Choices != null)
            {
                if (!binding.Parameter.Choices.Cast<object>().Contains(binding.Value))
                {
                    yield return "Not a valid choice";
                }
            }
            var queryChoices = binding.Parameter.QueryChoices();
            if (queryChoices != null)
            {
                var queryResult = (IEnumerable<string>) queryChoices.Invoke(new Dictionary<string, object>(){ {"query", binding.Value}});
                if (queryResult.Contains(binding.Value) == false)
                {
                    yield return "Not a valid choice";
                }
            }
        }


        private static object DoInvoke(IInvokeable invokeable, IDictionary<string, object> args)
        {
            return invokeable.Invoke(args);
        }

         

    }
}