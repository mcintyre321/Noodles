using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{
    public static class NodePropertiesRuleRegistry
    {
        /// <returns>
        /// true if the Property should defs be shown
        /// false if the Property should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowPropertyRule(object target, PropertyInfo PropertyInfo);

        public static ShowPropertyRule HideHideAttributedProperties = (t, mi) => mi.GetCustomAttributes(typeof(HideAttribute), true).Any() ? false : null as bool?;
        public static ShowPropertyRule HideGetChildProperties = (t, mi) => (t is IGetChild && mi.Name == "Item") ? false : null as bool?;
        public static ShowPropertyRule HideUndercoredProperties = (t, mi) => ShowByDefault && mi.Name.StartsWith("_") ? false : null as bool?;
        public static ShowPropertyRule HidePropertiesetters = (t, mi) => mi.Name.StartsWith("set_") ? false : null as bool?;
        public static ShowPropertyRule HideSystemObjectMembers = (t, mi) => mi.DeclaringType == typeof(System.Object) ? false : null as bool?;
        public static ShowPropertyRule ShowAttributedProperties = (t, mi) =>
        {
            if (ShowByDefault == false && mi.GetCustomAttributes(typeof(ShowAttribute), true).Any())
            {
                return true;
            }
            return null as bool?;
        };
        public static ShowPropertyRule ShowAttributedPropertiesetters = (t, mi) =>
        {
            if (mi.Name.StartsWith("get_"))
            {
                var pi = mi.DeclaringType.GetProperty(mi.Name.Substring(4));
                if (ShowByDefault == false && pi.GetCustomAttributes(typeof (ShowAttribute), true).Any())
                {
                    return true;
                }
            }
            return null as bool?;
        };

        public static ShowPropertyRule ClassLevelShowByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;
        public static ShowPropertyRule ClassLevelHideByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(HideAttribute), true).Any() ? false : null as bool?;

        public static bool ShowByDefault { get; set; }
        public static List<ShowPropertyRule> ShowPropertyRules { get; private set; }

         
        static NodePropertiesRuleRegistry()
        {
            ShowPropertyRules = new List<ShowPropertyRule>()
                                  {
                                      HideHideAttributedProperties,
                                      HideGetChildProperties,
                                      HideUndercoredProperties,
                                      HidePropertiesetters,
                                      HideSystemObjectMembers,
                                      ShowAttributedProperties,
                                      ShowAttributedPropertiesetters,
                                      ClassLevelShowByDefault,
                                      ClassLevelHideByDefault,
                                  };
        }

    }
}