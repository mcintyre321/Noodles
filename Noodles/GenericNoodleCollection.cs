using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noodles
{
    public class GenericNoodleCollection<TParent> : IHasParent<TParent>, IHasName, IHasChildren
    {
        private readonly string _name;
        private readonly Func<string, object> _tryGetChild;

        public GenericNoodleCollection(TParent user, string name, Func<string, object> tryGetChild)
        {
            _name = name;
            _tryGetChild = tryGetChild;
            Parent = user;
        }

        public TParent Parent { get; private set; }

        public string Name
        {
            get { return _name; }
        }

        public object GetChild(string name)
        {
            return _tryGetChild(name);
        }
    }
}
