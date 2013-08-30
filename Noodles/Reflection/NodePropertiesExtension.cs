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
        public static IEnumerable<INode> GetNodeProperties<TNode>(this object o, TNode resource, Type fallback = null) where TNode : INode
        {
            fallback = fallback ?? o.GetType();
            return YieldFindNodePropertiesUsingReflection(resource, o, fallback)
                .Concat(NodePropertiesAttribute.GetDynamicNodeProperties(o, resource))
                .Concat(BehaviourAttribute.GetBehaviourProperties(fallback, o, resource));
        }

       
        public static INode NodeProperty<TNode>(this object o, TNode resource, string propertyName, Type fallback = null) where TNode : INode
        {
            return o.GetNodeProperties(resource, fallback).SingleOrDefault(m => m.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
        }

        public static IEnumerable<INode> YieldFindNodePropertiesUsingReflection<TNode>(TNode node, object target, Type fallback)where TNode : INode
        {
            return YieldFindPropertyInfosUsingReflection(target, fallback).Select(pi => NodeProperty(node, target, pi));
        }

        private static INode NodeProperty<TNode>(TNode node, object target, PropertyInfo pi) where TNode : INode
        {
            var atts = pi.Attributes().OfType<ShowAttribute>();
            var getter = pi.GetGetMethod();
            if (getter != null) atts = atts.Concat(getter.Attributes().OfType<ShowAttribute>());
            //var showCollectionAttribute = atts.SingleOrDefault() as ShowCollectionAttribute;
            //if (showCollectionAttribute != null)
            //{
            //    var type = showCollectionAttribute.ColType ?? pi.PropertyType.GenericTypeArguments.First();
            //    return new NodeCollectionProperty<TNode>(node, target, pi, type);
            //}
            return new ReflectionNodeProperty(node, target, pi);
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
                if (ruleResult ?? false)
                {
                    yield return info;
                }
            }
        }
       
    }
}