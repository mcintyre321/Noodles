using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class ShowAttribute : Attribute { }
    public class HideAttribute : Attribute { }
    public static class NodeActionsRuleRegistry
    {
        /// <returns>
        /// true if the method should defs be shown
        /// false if the method should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowActionRule(object target, MethodInfo methodInfo);

        public static ShowActionRule ShowAttributedActions = (t, mi) => mi.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;
        public static ShowActionRule HideHideAttributedActions = (t, mi) => mi.GetCustomAttributes(typeof(HideAttribute), true).Any() ? false : null as bool?;
        public static ShowActionRule HideValuesActions = (t, mi) => mi.Name.EndsWith("_values", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowActionRule HideValueActions = (t, mi) => mi.Name.ToLowerInvariant() != "set_value" && mi.Name.EndsWith("_value", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowActionRule HideChoiceActions = (t, mi) => mi.Name.EndsWith("_choices", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowActionRule HideSuggestionsActions = (t, mi) => mi.Name.EndsWith("_suggestions", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowActionRule HideGetChildActions = (t, mi) => (t is IHasChildren && mi.Name == "GetChild") ? false : null as bool?;
        public static ShowActionRule HideHasNodeActions = (t, mi) => (t is IHasNodeActions && mi.Name == "NodeActions") ? false : null as bool?;
        public static ShowActionRule HideUndercoredActions = (t, mi) => mi.Name.StartsWith("_") ? false : null as bool?;
        public static ShowActionRule HidePropertyGetters = (t, mi) => mi.Name.StartsWith("get_") ? false : null as bool?;
        public static ShowActionRule HideSystemObjectMembers = (t, mi) => mi.DeclaringType == typeof(System.Object) ? false : null as bool?;
        public static ShowActionRule ClassLevelShowByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;
        public static ShowActionRule ClassLevelHideByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(HideAttribute), true).Any() ? false : null as bool?;


        static NodeActionsRuleRegistry()
        {
            ShowActionRules = new List<ShowActionRule>()
                                  {
                                      ShowAttributedActions,
                                      HideHideAttributedActions,
                                      HideValuesActions,
                                      HideValueActions,
                                      HideChoiceActions,
                                      HideSuggestionsActions,
                                      HideGetChildActions,
                                      HideHasNodeActions,
                                      HideUndercoredActions,
                                      HidePropertyGetters,
                                      HideSystemObjectMembers,
                                      ClassLevelShowByDefault,
                                      ClassLevelHideByDefault,
                                  };
        }

        public static bool ShowByDefault { get; set; }
        public static List<ShowActionRule> ShowActionRules { get; private set; }
        
        public static NodeActions GetNodeActions(this object o)
        {
            return new NodeActions(o);
        }
    }
}