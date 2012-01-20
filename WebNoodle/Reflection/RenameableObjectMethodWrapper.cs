using System.Collections.Generic;

namespace WebNoodle.Reflection
{
    public class RenameableObjectMethodWrapper : IObjectMethod
    {
        private readonly IObjectMethod _inner;
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

        public IEnumerable<ObjectMethodParameter> Parameters
        {
            get { return _inner.Parameters; }
        }

        public object Target
        {
            get { return _inner.Target; }
        }

        public void Invoke(object[] parameters)
        {
            _inner.Invoke(parameters);
        }

        public RenameableObjectMethodWrapper(IObjectMethod inner)
        {
            _inner = inner;
        }


    }
}