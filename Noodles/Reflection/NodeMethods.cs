using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{
    public delegate IEnumerable<NodeMethod> FindNodeMethodsRule(NodeMethods obj);
    public class NodeMethods : IEnumerable<NodeMethod>, Walkies.IGetChild
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
            Parent = node.SetChild(this, "actions");
        }

        object IGetChild.this[string name]
        {
            get { return Parent.NodeMethods().SingleOrDefault(nm => nm.Name == name); }
        }

        public IEnumerator<NodeMethod> GetEnumerator()
        {
            return FindNodeMethodsRules.SelectMany(r => r(this) ?? new NodeMethod[]{}).Each(nm => nm.SetParent(this, nm.Name)).GetEnumerator();
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