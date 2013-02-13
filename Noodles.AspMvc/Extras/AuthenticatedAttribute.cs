using System;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Noodles.AspMvc.Extras
{
    public class AuthenticatedAttribute : Attribute
    {
        public static bool? HardenRule(object o, MethodInfo mi)
        {
            if (mi.GetCustomAttributes(typeof(AuthenticatedAttribute), true).Any())
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    return false;
                }
            }
            return null;
        }
    }
}