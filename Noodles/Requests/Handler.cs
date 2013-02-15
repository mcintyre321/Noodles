using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Noodles.Requests.Results;

namespace Noodles.Requests
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

        
        public async Task<Result> HandleRequest(TContext cc, NoodlesRequest request, object root, string[] path, Func<IInvokeable, object[], object> doInvoke = null)
        {
            var rootResource = Resource.CreateGeneric(root, null);
            rootResource.Url = request.RootUrl;
            var node = (INode) rootResource;
            foreach (var fragment in path)
            {
                var prev = node;
                node = node.GetChild(fragment);
                if (node == null) return new NotFoundResult(prev, fragment);
            }

            foreach (var processor in CustomProcessors.Concat(DefaultProcessors))
            {
                var processorResult = await processor(cc, request, node, doInvoke);
                if (processorResult != null) return processorResult;
            }

            return new BadRequestResult();
        }
    }
}
