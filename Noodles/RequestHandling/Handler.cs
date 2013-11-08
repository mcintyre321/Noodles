using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Noodles.Models;
using Noodles.RequestHandling.ResultTypes;
using Walkies;

namespace Noodles.RequestHandling
{
    public class Handler
    {
        public static Func<INode, bool> AllowGet = o => true;

    }

    public abstract class Handler<TContext>
    {

        static Handler() { Noodles.Configuration.Initialise(); } //ensure the config has been run

        readonly List<RequestProcessor<TContext>> CustomProcessors = new List<RequestProcessor<TContext>>();

        public static List<RequestProcessor<TContext>> DefaultProcessors = new List<RequestProcessor<TContext>>()
        {
            DefaultProcessors<TContext>.ProcessInvoke,
            DefaultProcessors<TContext>.Read,
        };

        
        public async Task<Result> HandleRequest(TContext cc, RequestInfo requestInfo, object root, string[] path, Func<IInvokeable, IDictionary<string, object>, object> doInvoke = null)
        {
            var rootResource = ResourceFactory.Instance.Create(root, null, null);
            NoodlesContext.SetValue("Resource-" + root.GetHashCode(), rootResource);
            NoodlesContext.SetValue("Slug-" + root.GetHashCode(), requestInfo.RootUrl);
            var node = rootResource;
            foreach (var fragment in path)
            {
                var prev = node;
                node = node.GetChild(fragment);
                if (node == null || !Handler.AllowGet(node)) return new NotFoundResult(prev, fragment);
                NoodlesContext.SetValue("Resource-" + node.Target.GetHashCode(), node);
                NoodlesContext.SetValue("Slug-" + node.Target.GetHashCode(), fragment);
            }

            foreach (var processor in CustomProcessors.Concat(DefaultProcessors))
            {
                var processorResult = await processor(cc, requestInfo, node, doInvoke);
                if (processorResult != null) return processorResult;
            }

            return new BadRequestResult();
        }
    }

    


}
