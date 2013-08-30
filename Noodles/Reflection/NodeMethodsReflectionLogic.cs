using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;


namespace Noodles
{
    public class NodeMethodsReflectionLogic
    {

        public static IEnumerable<NodeMethod> YieldFindNodeMethodsUsingReflection(object target, INode resource)
        {
            const BindingFlags publicInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var type = target.GetType();
            var methods = type.GetMethods(publicInstanceBindingFlags).ToArray();
            foreach (var info in methods)
            {
                bool? ruleResult = null;
                foreach (var rule in NodeMethodsRuleRegistry.ShowMethodRules)
                {
                    ruleResult = rule(target, info);
                    if (ruleResult.HasValue) break;
                }
                if (ruleResult ?? false)
                {
                    yield return new ReflectionNodeMethod(resource, target, info);
                }
            }

            foreach (var nm in BehaviourAttribute.GetBehaviourMethods(type, target, resource))
            {
                yield return nm;
            }
        }
    }
}