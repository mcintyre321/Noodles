using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{
    public static class NodeFieldsRuleRegistry
    {
        /// <returns>
        /// true if the Field should defs be shown
        /// false if the Field should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowFieldRule(object target, FieldInfo fieldInfo);

        public static ShowFieldRule HideHideAttributedFields = (t, mi) => mi.GetCustomAttributes(typeof(HideAttribute), true).Any() ? false : null as bool?;
        public static ShowFieldRule HideFieldsetters = (t, mi) => mi.Name.StartsWith("set_") ? false : null as bool?;
        public static ShowFieldRule HideFieldgetters = (t, mi) => mi.Name.StartsWith("get_") ? false : null as bool?;
        public static ShowFieldRule HideSystemObjectMembers = (t, mi) => mi.DeclaringType == typeof(System.Object) ? false : null as bool?;
        public static ShowFieldRule ShowAttributedFields = (t, mi) =>
        {
            if (ShowByDefault == false && mi.GetCustomAttributes(typeof(ShowAttribute), true).Any())
            {
                return true;
            }
            return null as bool?;
        };
        

        public static ShowFieldRule ClassLevelShowByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;
        public static ShowFieldRule ClassLevelHideByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(HideAttribute), true).Any() ? false : null as bool?;

        public static bool ShowByDefault { get; set; }
        public static List<ShowFieldRule> ShowFieldRules { get; private set; }


        static NodeFieldsRuleRegistry()
        {
            ShowFieldRules = new List<ShowFieldRule>()
            {
                HideHideAttributedFields,
                HideFieldsetters,
                HideSystemObjectMembers,
                ShowAttributedFields,
                ClassLevelShowByDefault,
                ClassLevelHideByDefault,
            };
        }

    }
}