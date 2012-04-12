using System.Collections.Generic;

namespace Noodles
{
    public class NodeMethodWrapper : INodeMethod
    {
        private readonly INodeMethod _inner;
        private string _innerName;
        private string _innerDisplayName;
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

        public IEnumerable<NodeMethodParameter> Parameters
        {
            get { return _inner.Parameters; }
        }

        public object Target
        {
            get { return _inner.Target; }
        }

        public object Parent { set; private get; }


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
        }

        string IHasPath.Path
        {
            get { return (Parent ?? (this.Target)).Path() + "actions/" + this.Name; }
        }
    }
} 