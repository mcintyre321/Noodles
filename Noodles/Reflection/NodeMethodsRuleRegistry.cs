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

        public static ShowMethodRule ShowAttributedMethods = (t, mi) => mi.Attributes().OfType<ShowAttribute>().Any() ? true : null as bool?;
     
        
        public static List<ShowMethodRule> ShowMethodRules { get; private set; }


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