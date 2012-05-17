using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public delegate IEnumerable<NodeMethod> FindNodeMethodsRule(NodeMethods obj);
    public class NodeMethods : IHasChildren, IEnumerable<NodeMethod>, IHasParent<object>, IHasName
    {
        static NodeMethods()
        {
            FindNodeMethodsRules = new List<FindNodeMethodsRule>()
            {
                UseIHasNodeMethod,
                FindNodeMethodsUsingReflection
            };
        }

        public static List<FindNodeMethodsRule> FindNodeMethodsRules { get; private set; }

        public static FindNodeMethodsRule UseIHasNodeMethod = nm => nm.Parent is IHasNodeMethods ? ((IHasNodeMethods)nm.Parent).NodeMethods() : null;

        public object Parent { get; private set; }

        public NodeMethods(object node)
        {
            Parent = node;
        }

        public object GetChild(string name)
        {
            return Parent.NodeMethods().SingleOrDefault(nm => nm.Name == name);
        }

        public IEnumerator<NodeMethod> GetEnumerator()
        {
            return FindNodeMethodsRules.SelectMany(r => r(this) ?? new NodeMethod[]{}).GetEnumerator();
        }

        public static readonly FindNodeMethodsRule FindNodeMethodsUsingReflection = (nm) => YieldFindNodeMethodsUsingReflection(nm);

        public static IEnumerable<NodeMethod> YieldFindNodeMethodsUsingReflection(NodeMethods nm)
        {
            var methods = nm.Parent.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            foreach (var info in methods)
            {
                bool? ruleResult = null;
                foreach (var rule in NodeMethodsRuleRegistry.ShowMethodRules)
                {
                    ruleResult = rule(nm.Parent, info);
                    if (ruleResult.HasValue) break;
                }
                if (ruleResult ?? NodeMethodsRuleRegistry.ShowByDefault)
                {
                    yield return new NodeMethod(nm.Parent, nm, info);
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