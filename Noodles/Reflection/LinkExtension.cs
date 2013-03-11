using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;
using Walkies;

namespace Noodles
{
    public static class NodeLinksExtension
    {
        public static IEnumerable<NodeLink> NodeLinks(this object o, Resource resource, Type fallback = null)
        {
            return YieldFindNodeLinksUsingReflection(resource, o, fallback);//.OrderBy(p => p.Order);
        }

        public static NodeLink NodeLinks(this object o, Resource resource, string propertyName, Type fallback = null)
        {
            return o.NodeLinks(resource, fallback).SingleOrDefault(m => m.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
        }

        public static IEnumerable<NodeLink> YieldFindNodeLinksUsingReflection(Resource parent, object target, Type fallback)
        {
            return YieldFindPropertInfosUsingReflection(target, fallback).Select(pi => NodeLink(parent, pi));
        }

        private static NodeLink NodeLink(Resource parent, PropertyInfo pi)
        {
            return new NodeLink(parent, pi);
        }

        public static IEnumerable<PropertyInfo> YieldFindPropertInfosUsingReflection(this object target, Type fallback)
        {

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var type = target == null ? fallback : target.GetType();
            var propertyInfos = type.GetProperties(bindingFlags).ToArray();
            foreach (var info in propertyInfos)
            {
                bool? ruleResult = null;
                foreach (var rule in NodeLinkRuleRegistry.ShowLinkRules)
                {
                    ruleResult = rule(target, info);
                    if (ruleResult.HasValue) break;
                }
                if (ruleResult ?? false)
                {
                    yield return info;
                }
            }
        }
       
    }
}