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
           

            var testedProperties = target.GetType().GetProperties().Select(p => new
                {
                    Property = p,
                    RuleResult = NodeLinkRuleRegistry.ShowLinkRules.Select(r => r(target, p)).FirstOrDefault(r => r != null)
                });

            var passedProperties = testedProperties.Where(q => q.RuleResult == true).Select(q => q.Property);

            foreach (var pi in passedProperties)
            {
                yield return new NodeLink(parent, pi.Name, pi.GetValue(target), pi.Attributes().OfType<LinkAttribute>().Single().UiHint);
            }
            var links = passedProperties
                .Select(pi => new
                {
                    Items = pi.GetValue(target) as IDictionary<string, object>,
                    LinksAttribute = pi.Attributes().OfType<LinksAttribute>().SingleOrDefault()
                })
                .Where(x => x.LinksAttribute != null)
                .SelectMany(x => x.Items.Select(i => new NodeLink(parent, i.Key, i.Value, x.LinksAttribute.UiHint)));
          
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

        public static IEnumerable<Tuple<PropertyInfo, LinkAttribute>> YieldFindLinkAttributedProperties(this object target, Type fallback)
        {
            return target.AllPropertyInfos(fallback)
                .Select(t => Tuple.Create(t, t.Attributes().OfType<LinkAttribute>().SingleOrDefault()))
                .Where(tuple => tuple.Item2 != null);

        }
    }
}