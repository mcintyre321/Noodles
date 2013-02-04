using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Noodles.WebApi
{
    public delegate Task<HttpResponseMessage> NoodlesHttpProcessor(HttpRequestMessage request, CancellationToken token, object target);
}