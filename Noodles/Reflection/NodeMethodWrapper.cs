using System;
using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public class NodeMethodWrapper : INodeMethod, IHasParent<NodeMethods>
    {
        private readonly INodeMethod _inner;
        private string _innerName;
        private string _innerDisplayName;
        private NodeMethods _parent;

        public string Name
        {
            get { return _innerName ?? _inner.Name; }
            set { _innerName = value; }
        }

        public string DisplayName
        {
            get { return _innerDisplayName ?? _inner.DisplayName; }
            set { _innerDisplayName = value; }
        }

        public string Message
        {
            get { return _inner.Message; }
        }

        public IEnumerable<NodeMethodParameter> Parameters
        {
            get { return _inner.Parameters.Select(TransformParameters); }
        }

        public object Target
        {
            get { return _inner.Target; }
        }

        public NodeMethods Parent { set { _parent  = value; } get { return _parent ?? _inner.Parent() as NodeMethods; } }


        public string SuccessMessage
        {
            get { return _inner.SuccessMessage; }
        }

        public void Invoke(object[] parameters)
        {
            _inner.Invoke(parameters);
        }

        public NodeMethodWrapper(INodeMethod inner)
        {
            _inner = inner;
            TransformParameters = s => s;
        }

        public Func<NodeMethodParameter, NodeMethodParameter> TransformParameters { get; set; }

    }
} 