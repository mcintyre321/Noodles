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
            foreach (var link in YieldFindLinkAttributedProperties(target, fallback).Select(pi => new NodeLink(parent, pi.Name, pi.GetValue(target))))
            {
                yield return link;
            }

            var links = target.GetType().GetProperties().Where(
                p => p.Attributes().OfType<LinksAttribute>().Any())
                .Select(pi => (IDictionary<string, object>) pi.GetValue(target))
                .SelectMany(dict => dict)
                .Select(pair => new NodeLink(parent, pair.Key, pair.Value));

            foreach (var link in links)
            {
                yield return link;
            }

        }

        public static IEnumerable<PropertyInfo> AllPropertyInfos(this object o, Type fallback)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var type = o == null ? fallback : o.GetType();
            var propertyInfos = type.GetProperties(bindingFlags).ToArray();
            return propertyInfos;
        }

        public static IEnumerable<PropertyInfo> YieldFindLinkAttributedProperties(this object target, Type fallback)
        {
            var propertyInfos = target.AllPropertyInfos(fallback);
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