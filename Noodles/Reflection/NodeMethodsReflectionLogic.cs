using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;


namespace Noodles
{
    public delegate IEnumerable<NodeMethod> FindNodeMethodsRule(NodeMethodsReflectionLogic obj);
    public class NodeMethodsReflectionLogic
    {
        public static List<FindNodeMethodsRule> FindNodeMethodsRules { get; private set; }

        public static IEnumerable<NodeMethod> YieldFindNodeMethodsUsingReflection(object target, INode resource)
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
                    yield return new ReflectionNodeMethod(resource, target, info);
                }
            }
        }
    }
}