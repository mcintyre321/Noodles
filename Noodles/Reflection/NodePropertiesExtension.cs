using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{
    public static class NodePropertiesExtension
    {
        public static IEnumerable<NodeProperty> NodeProperties(this object o)
        {
            return YieldFindNodePropertiesUsingReflection(o).Concat(YieldFindNodeFieldsUsingReflection(o));
        }

        public static NodeProperty NodeProperty(this object o, string propertyName)
        {
            return o.NodeProperties().SingleOrDefault(m => m.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
        }

        public static IEnumerable<NodeProperty> YieldFindNodePropertiesUsingReflection(object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var propertyInfos = target.GetType().GetProperties(bindingFlags).ToArray();
            foreach (var info in propertyInfos)
            {
                bool? ruleResult = null;
                foreach (var rule in NodePropertiesRuleRegistry.ShowPropertyRules)
                {
                    ruleResult = rule(target, info);
                    if (ruleResult.HasValue) break;
                }
                if (ruleResult ?? NodePropertiesRuleRegistry.ShowByDefault)
                {
                    yield return new NodeProperty(target, info);
                }
            }
        }
        public static IEnumerable<NodeProperty> YieldFindNodeFieldsUsingReflection(object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var propertyInfos = target.GetType().GetFields(bindingFlags).ToArray();
            foreach (var info in propertyInfos)
            {
                bool? ruleResult = null;
                foreach (var rule in NodeFieldsRuleRegistry.ShowFieldRules)
                {
                    ruleResult = rule(target, info);
                    if (ruleResult.HasValue) break;
                }
                if (ruleResult ?? NodeFieldsRuleRegistry.ShowByDefault)
                {
                    yield return new NodeProperty(target, info);
                }
            }
        }
    }
}