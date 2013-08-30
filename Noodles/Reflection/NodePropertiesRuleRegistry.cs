using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Noodles
{
    public static class NodePropertiesRuleRegistry
    {
        /// <returns>
        /// true if the Property should defs be shown
        /// false if the Property should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowPropertyRule(object target, PropertyInfo propertyInfo);

        public static ShowPropertyRule ShowAttributedProperties = (t, propertyInfo) =>
        {
            if (propertyInfo.GetCustomAttributes(typeof(ShowAttribute), true).Any())
            {
                return true;
            }
            return null as bool?;
        };
        public static ShowPropertyRule ShowAttributedPropertyGetters = (t, propertyInfo) =>
        {
            var methodInfo = propertyInfo.DeclaringType.GetMethod("get_" + propertyInfo.Name);
            if (methodInfo != null &&  methodInfo.GetCustomAttributes().OfType<ShowAttribute>().Any())
            {
                return true;
            }
            return null as bool?;
        };
        public static ShowPropertyRule ShowAttributedPropertySetters = (t, propertyInfo) =>
        {
            var methodInfo = propertyInfo.DeclaringType.GetMethod("set_" + propertyInfo.Name);
            if (methodInfo != null && methodInfo.Attributes().OfType<ShowAttribute>().Any())
            {
                return true;
            }
            return null as bool?;
        };


        public static ShowPropertyRule ClassLevelShowByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;

        public static List<ShowPropertyRule> ShowPropertyRules { get; private set; }

         
        static NodePropertiesRuleRegistry()
        {
            ShowPropertyRules = new List<ShowPropertyRule>()
                                  {
                                      ShowAttributedProperties,
                                      ShowAttributedPropertyGetters,
                                  };
        }

    }
}