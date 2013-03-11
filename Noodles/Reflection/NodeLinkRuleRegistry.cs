using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{
    public static class NodeLinkRuleRegistry
    {
        /// <returns>
        /// true if the Property should defs be shown
        /// false if the Property should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowLinkRule(object target, PropertyInfo propertyInfo);

        public static ShowLinkRule HideGetChildLinks = (t, propertyInfo) => (t is IGetChild && propertyInfo.Name == "Item") ? false : null as bool?;
        public static ShowLinkRule HideUndercoredLinks = (t, propertyInfo) => propertyInfo.Name.StartsWith("_") ? false : null as bool?;
        public static ShowLinkRule HideSystemObjectMembers = (t, propertyInfo) => propertyInfo.DeclaringType == typeof(System.Object) ? false : null as bool?;
        public static ShowLinkRule LinkAttributedLinks = (t, propertyInfo) =>
        {
            if (propertyInfo.GetCustomAttributes(typeof(LinkAttribute), true).Any())
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


        public static ShowLinkRule ClassLevelShowByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(LinkAttribute), true).Any() ? true : null as bool?;

        public static List<ShowLinkRule> ShowLinkRules { get; private set; }

         
        static NodeLinkRuleRegistry()
        {
            ShowLinkRules = new List<ShowLinkRule>()
                                  {
                                      LinkAttributedLinks,
                                      LinkAttributedPropertyGetters,
                                  };
        }

    }
}