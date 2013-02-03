using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Noodles.Example.WebApi
{
    public delegate Task<HttpResponseMessage> NoodlesHttpProcessor(HttpRequestMessage request, CancellationToken token, object target);
}