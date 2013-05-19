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
            var publicInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
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
                if (ruleResult ?? NodeMethodsRuleRegistry.ShowByDefault)
                {
                    yield return new ReflectionNodeMethod(resource, target, info);
                }
            }
            var behaviourProperties = type.GetProperties(publicInstanceBindingFlags | BindingFlags.NonPublic)
                                            .Where(pi => pi.Attributes().OfType<BehaviourAttribute>().Any());
            foreach (var propertyInfo in behaviourProperties)
            {
                var behaviour = propertyInfo.GetValue(target);
                foreach (var nodeMethod in YieldFindNodeMethodsUsingReflection(behaviour, resource))
                {
                    yield return nodeMethod;
                }
            }
            var behaviourFields = type.GetFields(publicInstanceBindingFlags | BindingFlags.NonPublic | BindingFlags.Instance).Where(pi => pi.Attributes().OfType<BehaviourAttribute>().Any());
            foreach (var fieldInfo in behaviourFields)
            {
                var behaviour = fieldInfo.GetValue(target);
                foreach (var nodeMethod in YieldFindNodeMethodsUsingReflection(behaviour, resource))
                {
                    yield return nodeMethod;
                }
            }

        }
    }
}