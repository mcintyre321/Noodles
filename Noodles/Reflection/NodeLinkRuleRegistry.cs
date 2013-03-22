using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{
    public static class NodeLinkRuleRegistry
    {
        static NodeLinkRuleRegistry()
        {
            ShowLinkRules = new List<ShowLinkRule>()
                {
                    LinkAttributedProperties,
                    LinkAttributedPropertyGetters,
                    LinkAttributedPropertySetters,
                };

        }
        /// <returns>
        /// true if the Property should defs be shown
        /// false if the Property should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowLinkRule(object target, PropertyInfo propertyInfo);

        public static ShowLinkRule LinkAttributedProperties = (t, propertyInfo) =>
        {
            if (propertyInfo.Attributes().OfType<LinkAttribute>().Any())
            {
                return true;
            }
            return null as bool?;
        };
        public static ShowLinkRule LinkAttributedPropertyGetters = (t, propertyInfo) =>
        {
            var methodInfo = propertyInfo.DeclaringType.GetMethod("get_" + propertyInfo.Name);
            if (methodInfo != null && methodInfo.GetCustomAttributes(typeof (LinkAttribute), true).Any())
            {
                return true;
            }
            return null as bool?;
        };
        public static ShowLinkRule LinkAttributedPropertySetters = (t, propertyInfo) =>
        {
            var methodInfo = propertyInfo.DeclaringType.GetMethod("set_" + propertyInfo.Name);
            if (methodInfo != null && methodInfo.GetCustomAttributes(typeof (LinkAttribute), true).Any())
            {
                return true;
            }
            return null as bool?;
        };

        public static IList<ShowLinkRule> ShowLinkRules { get; private set; }
    }
}