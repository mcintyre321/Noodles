using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;


namespace Noodles
{
    public static class NodePropertiesExtension
    {
        public static IEnumerable<NodeProperty> GetNodeProperties<TNode>(this object o, TNode resource, Type fallback = null) where TNode : INode
        {
            return YieldFindNodePropertiesUsingReflection(resource, o, fallback).OrderBy(p => p.Order)
                .Concat(NodePropertiesAttribute.GetDynamicNodeProperties(o, resource));
        }

       
        public static NodeProperty NodeProperty<TNode>(this object o, TNode resource, string propertyName, Type fallback = null) where TNode : INode
        {
            return o.GetNodeProperties(resource, fallback).SingleOrDefault(m => m.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
        }

        public static IEnumerable<NodeProperty> YieldFindNodePropertiesUsingReflection<TNode>(TNode node, object target, Type fallback)where TNode : INode
        {
            return YieldFindPropertyInfosUsingReflection(target, fallback).Select(pi => NodeProperty(node, target, pi));
        }

        private static NodeProperty NodeProperty<TNode>(TNode node, object target, PropertyInfo pi) where TNode : INode
        {
            var atts = pi.Attributes().OfType<ShowAttribute>();
            var getter = pi.GetGetMethod();
            if (getter != null) atts = atts.Concat(getter.Attributes().OfType<ShowAttribute>());
            var showCollectionAttribute = atts.SingleOrDefault() as ShowCollectionAttribute;
            if (showCollectionAttribute != null)
            {
                var type = showCollectionAttribute.ColType ?? pi.PropertyType.GenericTypeArguments.First();
                return new NodeCollectionProperty<TNode>(node, target, pi, type);
            }
            return new ReflectionNodeProperty<TNode>(node, target, pi);
        }

        public static IEnumerable<PropertyInfo> YieldFindPropertyInfosUsingReflection(this object target, Type fallback)
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