using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;

namespace Noodles
{
    public class BehaviourAttribute : Attribute
    {
        public static IEnumerable<INode> GetBehaviourProperties(Type t, object o, INode resource)
        {
            foreach (var behaviour in GetBehavioursFromProperties(t, o))
            {
                foreach (var nodeMethod in behaviour.GetNodeProperties(resource))
                {
                    yield return nodeMethod;
                }
            }

            var behaviourFields = GetBehavioursFromFields(t, o);
            foreach (var behaviour in behaviourFields)
            {
                foreach (var nodeMethod in behaviour.GetNodeProperties(resource))
                {
                    yield return nodeMethod;
                }
            }
        }

        public static IEnumerable<NodeMethod> GetBehaviourMethods(Type type, object target, INode resource)
        {
    
            foreach (var behaviour in GetBehavioursFromProperties(type, target))
            {
                foreach (var nodeMethod in NodeMethodsReflectionLogic.YieldFindNodeMethodsUsingReflection(behaviour, resource))
                {
                    yield return nodeMethod;
                }
            }

            var behaviourFields = GetBehavioursFromFields(type, target);
            foreach (var behaviour in behaviourFields)
            {
                foreach (var nodeMethod in NodeMethodsReflectionLogic.YieldFindNodeMethodsUsingReflection(behaviour, resource))
                {
                    yield return nodeMethod;
                }
            }
        }

        private static IEnumerable<object> GetBehavioursFromFields(Type type, object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance;
            
            var behaviourFields = type.GetFields(bindingFlags)
                                      .Where(pi => pi.Attributes().OfType<BehaviourAttribute>().Any())
                                      .Select(pi => pi.GetValue(target));
            return behaviourFields;
        }

        private static IEnumerable<object> GetBehavioursFromProperties(Type type, object target)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic;

            var behaviourProperties = type.GetProperties(bindingFlags)
                                          .Where(pi => pi.Attributes().OfType<BehaviourAttribute>().Any())
                                          .Select(pi => pi.GetValue(target));
            return behaviourProperties;
        }
    }
}