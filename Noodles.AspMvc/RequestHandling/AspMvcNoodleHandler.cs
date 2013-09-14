using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FormFactory;
using FormFactory.AspMvc.ModelBinders;
using FormFactory.AspMvc.Wrappers;
using FormFactory.ModelBinding;
using Noodles.AspMvc.RequestHandling.Transforms;
using Noodles.Models;
using Noodles.RequestHandling;

namespace Noodles.AspMvc.RequestHandling
{
    public class AspMvcNoodleHandler : Handler<ControllerContext>
    {
        public static List<Func<ControllerContext, object, ActionResult>> Processors = new List<Func<ControllerContext, object, ActionResult>>();
        static List<Func<Exception, ControllerContext, Action>> ModelStateExceptionHandlers = new List<Func<Exception, ControllerContext, Action>>();
        private TransformRuleRegistry _ruleRegistry;

        public static void AddExceptionHandler<TEx>(Action<TEx, ControllerContext> action) where TEx : Exception
        {
            ModelStateExceptionHandlers.Add((e, msd) => (e as TEx) == null ? null as Action : () => action((TEx)e, msd));
            ModelStateExceptionHandlers.Add((e, msd) => (e as NodeNotFoundException) == null ? null as Action : () => action((TEx)e, msd));
        }

        static AspMvcNoodleHandler()
        {
            //AddExceptionHandler<UserException>((e, nodeLink) => nodeLink.Controller.ViewData.ModelState.AddModelError("", e));
            var propertyVms = VmHelper.GetPropertyVms;
            VmHelper.GetPropertyVms = (h, o, a) => GetPropertyVms(new Encoder(), o, a, propertyVms);
        }

        public AspMvcNoodleHandler(TransformRuleRegistry ruleRegistry)
        {
            _ruleRegistry = ruleRegistry;
        }


        private static IEnumerable<PropertyVm> GetPropertyVms(IStringEncoder encoder, object o, Type type, Func<IStringEncoder, object, Type, IEnumerable<PropertyVm>> fallback)
        {
            if (o == null) o = Activator.CreateInstance(type);

            yield return new PropertyVm(typeof(string), "__type")
            {
                IsHidden = true,
                Value = encoder.WriteTypeToString(o.GetType())
            };

            var propertyVms = o.YieldFindPropertyInfosUsingReflection(type).Select(p => new PropertyVm(o, p));
            foreach (var propertyVm in propertyVms)
            {
                yield return propertyVm;
            }
        }

        public async Task<ActionResult> GetNoodleResult(ControllerContext cc, object root, string path = null, Func<IInvokeable, IDictionary<string, object>, object> doInvoke = null)
        {
            var nr = new AspMvcRequestInfo(cc);
            var handler = new AspMvcNoodleHandler(_ruleRegistry);
            var pathParts = (path ?? cc.RouteData.Values["path"] as string ?? "/").Trim('/')
                .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var noodleResponse = await handler.HandleRequest(cc, nr, root, pathParts, doInvoke);

            var transformer = new NoodleResultToActionResultMapper(_ruleRegistry);
            return transformer.Map(cc, noodleResponse);
        }
    }
}