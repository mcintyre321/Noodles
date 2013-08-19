using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


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
        /// <summary>
        /// Use this if the type of the collection cannot be figured out from the properties generic argument 
        /// </summary>
        public Type ColType { get; set; }
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

        static NodeMethodsRuleRegistry()
        {
            ShowMethodRules = new List<ShowMethodRule>()
            {
                ShowAttributedMethods,
            };
        }

        internal static U Maybe<T, U>(this T t, Func<T, U> f) where T : class
        
        {
            return (t == null) ? default(U) : f(t);
        }
    }
}