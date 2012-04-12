using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class NodeActions : IHasChildren , IEnumerable<INodeAction>
    {
        public object Node { get; private set; }

        public NodeActions(object node)
        {
            Node = node;
        }

        public object GetChild(string name)
        {
            return Node.NodeActions().SingleOrDefault(nm => nm.Name == name);
        }

        public IEnumerator<INodeAction> GetEnumerator()
        {
            var methods = Node.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            var filteredMethods = methods
                .Where(mi => NodeActionsRuleRegistry.ShowActionRules.Select(mf => mf(Node, mi)).FirstOrDefault(
                    show => show != null) ?? NodeActionsRuleRegistry.ShowByDefault)
                .Select(mi => new NodeAction(Node, mi));
            return filteredMethods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}