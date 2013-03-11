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
        public static IEnumerable<NodeProperty> NodeProperties(this object o, INode resource, Type fallback = null)
        {
            return YieldFindNodePropertiesUsingReflection(resource, o, fallback).OrderBy(p => p.Order);
        }

        public static NodeProperty NodeProperty(this object o, INode resource, string propertyName, Type fallback = null)
        {
            return o.NodeProperties(resource, fallback).SingleOrDefault(m => m.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
        }

        public static IEnumerable<NodeProperty> YieldFindNodePropertiesUsingReflection(INode node, object target, Type fallback)
        {
            return YieldFindPropertInfosUsingReflection(target, fallback).Select(pi => NodeProperty(node, target, pi));
        }

        private static NodeProperty NodeProperty(INode node, object target, PropertyInfo pi)
        {
            if (pi.Attributes().OfType<ShowAttribute>().SingleOrDefault() as ShowCollectionAttribute != null)
            {
                return new NodeCollectionProperty(node, target, pi);
            }
            return new NodeProperty(node, target, pi);
        }

        static IEnumerable<PropertyInfo> YieldFindPropertInfosUsingReflection(this object target, Type fallback)
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