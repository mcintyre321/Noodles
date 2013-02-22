using System;
using System.Threading.Tasks;
using Noodles.Models;
using Noodles.Requests.Results;

namespace Noodles.Requests
{
    public delegate Task<Result> RequestProcessor<TContext>(TContext context, NoodlesRequest request, INode node, Func<IInvokeable, object[], object> doInvoke);
}