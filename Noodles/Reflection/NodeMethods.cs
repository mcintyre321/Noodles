using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class NodeMethods : IHasChildren, IEnumerable<INodeMethod>, IHasParent<object>, IHasName
    {
        public object Parent { get; private set; }

        public NodeMethods(object node)
        {
            Parent = node;
        }

        public object GetChild(string name)
        {
            return Parent.NodeMethods().SingleOrDefault(nm => nm.Name == name);
        }

        public IEnumerator<INodeMethod> GetEnumerator()
        {
            var methods = Parent.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            var filteredMethods = methods
                .Where(mi => NodeMethodsRuleRegistry.ShowMethodRules.Select(mf => mf(Parent, mi)).FirstOrDefault(
                    show => show != null) ?? NodeMethodsRuleRegistry.ShowByDefault)
                .Select(mi => new NodeMethod(Parent, this, mi));
            return filteredMethods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Name
        {
            get { return "actions"; }
        }
    }
}