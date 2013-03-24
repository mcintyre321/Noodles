using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles
{

  
    public class UiHintException : Exception
    {
        public UiHintException(string message):base(message)
        {
            
        }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class ShowAttribute : Attribute
    {
        public ShowAttribute()
        {
            UiOrder = int.MaxValue;
        }
        public string UiHint { get; set; }
        public int UiOrder { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class ShowCollectionAttribute : ShowAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AutoSubmitAttribute : Attribute
    {
        public bool AutoSubmit { get; private set; }

        public AutoSubmitAttribute(bool autoSubmit = true)
        {
            AutoSubmit = autoSubmit;
        }
    }
    public static class NodeMethodsRuleRegistry
    {
        /// <returns>
        /// true if the method should defs be shown
        /// false if the method should defs be hidden
        /// null when not sure
        /// </returns>
        public delegate bool? ShowMethodRule(object target, MethodInfo methodInfo);

        public static ShowMethodRule ShowAttributedMethods = (t, mi) =>
        {
            if (ShowByDefault == false && mi.GetCustomAttributes(typeof(ShowAttribute), true).Any())
            {
                return true;
            }
            return null as bool?;
        };
     
        

        public static ShowMethodRule ClassLevelShowByDefault = (t, mi) => mi.DeclaringType.GetCustomAttributes(typeof(ShowAttribute), true).Any() ? true : null as bool?;

        public static bool ShowByDefault { get; set; }
        public static List<ShowMethodRule> ShowMethodRules { get; private set; }


        /// <returns>
        /// true if the method should defs be auto-submitted
        /// false if the method should defs not be auto-submitted
        /// null when not sure
        /// </returns>
        public delegate bool? AutoSubmitRule(MethodInfo methodInfo);
        public static bool AutoSubmitByDefault { get; set; }
        public static List<AutoSubmitRule> AutoSubmitRules { get; private set; }

        public static AutoSubmitRule NoAutoSubmitWhenHasParams = (mi) => mi.GetParameters().Any() ? false : null as bool?;
        public static AutoSubmitRule AutoSubmitAttribute = (mi) => mi.GetCustomAttributes(typeof(AutoSubmitAttribute), true).Cast<AutoSubmitAttribute>().FirstOrDefault().Maybe(x => x.AutoSubmit as bool?);

        static NodeMethodsRuleRegistry()
        {
            ShowMethodRules = new List<ShowMethodRule>() { ShowAttributedMethods, };
            AutoSubmitRules = new List<AutoSubmitRule>
                                  {
                                      NoAutoSubmitWhenHasParams,
                                      AutoSubmitAttribute
                                  };
        }

        internal static U Maybe<T, U>(this T t, Func<T, U> f) where T : class
        
        {
            return (t == null) ? default(U) : f(t);
        }
    }
}