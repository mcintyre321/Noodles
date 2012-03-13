using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noodles
{
    public class ShowAttribute : Attribute{}
    public class HideAttribute : Attribute { }
    public static class NodeMethodsExtension
    {
        /// <returns>
        /// true if the method should defs be shown
        /// false if the method should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowMethodRule(object target, MethodInfo methodInfo);

        public static ShowMethodRule ShowAttributedMethods = (t, mi) => mi.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;
        public static ShowMethodRule HideHideAttributedMethods = (t, mi) => mi.GetCustomAttributes(typeof(HideAttribute), true).Any() ? false : null as bool?;
        public static ShowMethodRule HideValuesMethods = (t, mi) => mi.Name.EndsWith("_values", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowMethodRule HideValueMethods = (t, mi) => mi.Name.ToLowerInvariant() != "set_value" && mi.Name.EndsWith("_value", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowMethodRule HideChoiceMethods = (t, mi) => mi.Name.EndsWith("_choices", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowMethodRule HideSuggestionsMethods = (t, mi) => mi.Name.EndsWith("_suggestions", StringComparison.InvariantCultureIgnoreCase) ? false : null as bool?; //choices should be hidden
        public static ShowMethodRule HideGetChildMethods = (t, mi) => (t is IHasChildren && mi.Name == "GetChild") ? false : null as bool?;
        public static ShowMethodRule HideHasNodeMethods = (t, mi) => (t is IHasNodeMethods && mi.Name == "NodeMethods") ? false : null as bool?;
        public static ShowMethodRule HideUndercoredMethods = (t, mi) => mi.Name.StartsWith("_") ? false : null as bool?;
        public static ShowMethodRule HidePropertyGetters = (t, mi) => mi.Name.StartsWith("get_") ? false : null as bool?;
        public static ShowMethodRule HideSystemObjectMembers = (t, mi) => mi.DeclaringType == typeof(System.Object) ? false : null as bool?;
        public static ShowMethodRule ClassLevelShowByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;

        static NodeMethodsExtension()
        {
            ShowMethodRules = new List<ShowMethodRule>()
            {
                ShowAttributedMethods,
                HideValuesMethods,
                HideValueMethods,
                HideChoiceMethods,
                HideSuggestionsMethods,
                HideGetChildMethods,
                HideHasNodeMethods,
                HideUndercoredMethods,
                HidePropertyGetters,
                HideSystemObjectMembers,
                ClassLevelShowByDefault
            };
        }

        public static bool ShowByDefault { get; set; }
        public static List<ShowMethodRule> ShowMethodRules { get; private set; }
        
        public static IEnumerable<IObjectMethod> GetNodeMethodInfos(this object o)
        {
            var methods = o.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToArray();
            return methods
                .Where(mi => ShowMethodRules.Select(mf => mf(o, mi)).FirstOrDefault(show => show != null) ?? ShowByDefault)
                .Select(mi => new ObjectMethod(o, mi))
                .ToArray();
        }
    }
}