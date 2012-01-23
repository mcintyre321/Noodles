using System.Collections.Generic;

namespace WebNoodle.Reflection
{
    public class ObjectMethodWrapper : IObjectMethod, IHasPath
    {
        private readonly IObjectMethod _inner;
        private string _innerName;
        private string _innerDisplayName;
        private string _path;

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

        public ObjectMethodWrapper(IObjectMethod inner)
        {
            _inner = inner;
        }


        public string Path
        {
            get { return _path ?? this.Target.Path(); }
            set { _path = Path; }
        }
    }
}