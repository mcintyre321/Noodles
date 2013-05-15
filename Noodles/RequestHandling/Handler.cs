using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Noodles.Models;
using Noodles.RequestHandling.ResultTypes;

namespace Noodles.RequestHandling
{
    public abstract class Handler<TContext>
    {
        static Handler() { Noodles.Configuration.Initialise(); } //ensure the config has been run
        
        public List<RequestProcessor<TContext>> CustomProcessors = new List<RequestProcessor<TContext>>();

        
        public static List<RequestProcessor<TContext>> DefaultProcessors = new List<RequestProcessor<TContext>>()
        {
            DefaultProcessors<TContext>.ProcessInvoke,
            DefaultProcessors<TContext>.Read,
        };

        
        public async Task<Result> HandleRequest(TContext cc, RequestInfo requestInfo, object root, string[] path, Func<IInvokeable, IDictionary<string, object>, object> doInvoke = null)
        {
            var rootResource = ResourceFactory.Instance.Create(root, null, null);
            rootResource.RootUrl = requestInfo.RootUrl;
            var node = (INode) rootResource;
            foreach (var fragment in path)
            {
                var prev = node;
                node = node.GetChild(fragment);
                if (node == null) return new NotFoundResult(prev, fragment);
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
