using System;
using System.Threading.Tasks;
using Noodles.Models;
using Noodles.RequestHandling.ResultTypes;

namespace Noodles.RequestHandling
{
    public delegate Task<Result> RequestProcessor<TContext>(TContext context, NoodlesRequest request, INode node, Func<IInvokeable, object[], object> doInvoke);
}