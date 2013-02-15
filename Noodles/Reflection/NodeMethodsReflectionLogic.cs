using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Requests;
using Walkies;

namespace Noodles
{
    public interface IHasNodeMethods
    {
        IEnumerable<NodeMethod> NodeMethods();
    }
    public delegate IEnumerable<NodeMethod> FindNodeMethodsRule(NodeMethodsReflectionLogic obj);
    public class NodeMethodsReflectionLogic
    {
        public static List<FindNodeMethodsRule> FindNodeMethodsRules { get; private set; }

        public static FindNodeMethodsRule UseIHasNodeMethod = nm => nm.Parent() is IHasNodeMethods ? ((IHasNodeMethods)nm.Parent()).NodeMethods() : null;
        public static IEnumerable<NodeMethod> YieldFindNodeMethodsUsingReflection(object target, Resource resource)
        {
            var methods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            foreach (var info in methods)
            {
                bool? ruleResult = null;
                foreach (var rule in NodeMethodsRuleRegistry.ShowMethodRules)
                {
                    ruleResult = rule(target, info);
                    if (ruleResult.HasValue) break;
                }
                if (ruleResult ?? NodeMethodsRuleRegistry.ShowByDefault)
                {
                    yield return new NodeMethod(resource, target, info);
                }
            }
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //public string Name
        //{
        //    get { return "actions"; }
        //}
    }
}