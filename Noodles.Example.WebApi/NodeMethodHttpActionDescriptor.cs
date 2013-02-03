using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Noodles.Example.WebApi
{
    internal class NodeMethodHttpActionDescriptor : HttpActionDescriptor
    {
        private readonly NodeMethod _nodeMethod;

        public NodeMethodHttpActionDescriptor(NodeMethod nodeMethod, HttpConfiguration configuration)
        {
            _nodeMethod = nodeMethod;
            Configuration = configuration;
        }

        #region Overrides of HttpActionDescriptor

        public override Collection<HttpParameterDescriptor> GetParameters()
        {
            var parameters = _nodeMethod.Parameters.Select(p => new NodeMethodHttpParameterDescriptor(p, Configuration, this));
            return new Collection<HttpParameterDescriptor>(new List<HttpParameterDescriptor>(parameters));
        }

        public override Task<object> ExecuteAsync(HttpControllerContext controllerContext, IDictionary<string, object> arguments, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(_nodeMethod.Invoke(arguments));
            return tcs.Task;
        }

        public override string ActionName
        {
            get { return _nodeMethod.Name; }
        }

        public override Type ReturnType
        {
            get { return _nodeMethod.ReturnType; }
        }

        #endregion
    }
}