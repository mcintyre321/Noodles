using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{
    public delegate IEnumerable<NodeProperty> FindNodePropertiesRule(NodePropertiesReflectionLogic obj);

    public class NodeProperty
    {
        private PropertyInfo _info;

        public NodeProperty(object target, PropertyInfo info)
        {
            _info = info;
            Value = info.GetValue(target, null);
            PropertyType = info.PropertyType;
            Name = info.Name;
            DisplayName = GetDisplayName(info);

        }

        public string DisplayName { get; private set; }
        public Type PropertyType { get; private set; }
        public object Value { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<object> CustomAttributes
        {
            get { return _info.GetCustomAttributes(); }
        }

        string GetDisplayName(PropertyInfo info)
        {
            var att = info.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (att == null)
            {
                return Name.Replace("_", "").Sentencise();
            }
            return att.DisplayName;
        }
    }

    public class NodePropertiesReflectionLogic
    {
        public static List<FindNodePropertiesRule> FindNodePropertiesRules { get; private set; }

        public static IEnumerable<NodeProperty> YieldFindNodePropertiesUsingReflection(object target)
        {
            var propertyInfos = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
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
                    yield return new NodeProperty(target, info);
                }
            }
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //public string Name
        //{
        //    get { return "actions"; }
        //}
    }
}