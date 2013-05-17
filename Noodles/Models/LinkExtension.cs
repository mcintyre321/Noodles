using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;
using Noodles.RequestHandling;


namespace Noodles
{
    public static class NodeLinksExtension
    {
        public static IEnumerable<NodeLink> NodeLinks(this object o, Resource resource, Type fallback = null)
        {
            return YieldFindNodeLinksUsingReflection(resource, o, fallback);//.OrderBy(p => p.Order);
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
                var linkAttribute = pi.Attributes().OfType<LinkAttribute>().SingleOrDefault();
                if (linkAttribute != null)
                {
                    yield return new ReflectionNodeLink(parent, pi.Name, pi.GetValue(target), linkAttribute.UiHint);
                }
                else
                {
                    var linksAttribute = pi.Attributes().OfType<LinksAttribute>().Single();
                    var items = (IDictionary<string, object>) pi.GetValue(target);
                    foreach (var i in items)
                    {
                        yield return new ReflectionNodeLink(parent, i.Key, i.Value, linksAttribute.UiHint);
                    }
                }
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