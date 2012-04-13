using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class NodeMethods : IHasChildren, IEnumerable<INodeMethod>
    {
        public object Node { get; private set; }

        public NodeMethods(object node)
        {
            Node = node;
        }

        public object GetChild(string name)
        {
            return Node.NodeMethods().SingleOrDefault(nm => nm.Name == name);
        }

        public IEnumerator<INodeMethod> GetEnumerator()
        {
            var methods = Node.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            var filteredMethods = methods
                .Where(mi => NodeMethodsRuleRegistry.ShowMethodRules.Select(mf => mf(Node, mi)).FirstOrDefault(
                    show => show != null) ?? NodeMethodsRuleRegistry.ShowByDefault)
                .Select(mi => new NodeMethod(Node, mi));
            return filteredMethods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}