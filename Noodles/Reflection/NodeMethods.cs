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
            List<INodeMethod> passedMethods = new List<INodeMethod>();
            return YieldNodeMethods().GetEnumerator();
        }

        IEnumerable<INodeMethod> YieldNodeMethods()
        {
            var methods = Parent.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            foreach (var info in methods)
            {
                bool? ruleResult = null;
                foreach (var rule in NodeMethodsRuleRegistry.ShowMethodRules)
                {
                    ruleResult = rule(Parent, info);
                    if (ruleResult.HasValue) break;
                }
                if (ruleResult ?? NodeMethodsRuleRegistry.ShowByDefault)
                {
                    yield return new NodeMethod(Parent, this, info);
                }
            }
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