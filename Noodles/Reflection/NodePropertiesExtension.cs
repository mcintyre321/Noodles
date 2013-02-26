using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;
using Walkies;

namespace Noodles
{
    public static class NodePropertiesExtension
    {
        public static IEnumerable<NodeProperty> NodeProperties(this object o, Resource resource, Type fallback = null)
        {
            return YieldFindNodePropertiesUsingReflection(resource, o, fallback).OrderBy(p => p.Order);
        }

        public static NodeProperty NodeProperty(this object o, Resource resource, string propertyName, Type fallback = null)
        {
            return o.NodeProperties(resource, fallback).SingleOrDefault(m => m.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
        }

        public static IEnumerable<NodeProperty> YieldFindNodePropertiesUsingReflection(Resource node, object target, Type fallback)
        {
            return
                YieldFindPropertInfosUsingReflection(target, fallback).Select(pi => new NodeProperty(node, target, pi));
        }
        public static IEnumerable<PropertyInfo> YieldFindPropertInfosUsingReflection(this object target, Type fallback)
        {

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var type = target == null ? fallback : target.GetType();
            var propertyInfos = type.GetProperties(bindingFlags).ToArray();
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
                    yield return info;
                }
            }
        }
       
    }
}