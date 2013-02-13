using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Security;

namespace Noodles.AspMvc.Extras
{
    public class RequireRoleAttribute : Attribute
    {
        public RequireRoleAttribute(params string[] roles)
        {
            this.RequiredRoles = roles.ToList().AsReadOnly();
        }

        public IEnumerable<string> RequiredRoles { get; private set; }

        public static bool? HardenRule(object o, MethodInfo mi)
        {
            var att = mi.GetCustomAttributes(typeof(RequireRoleAttribute), true).Cast<RequireRoleAttribute>().SingleOrDefault();
            if (att != null)
            {
                if (Roles.GetRolesForUser().Any(r => att.RequiredRoles.Contains(r)))
                {
                    return false;
                }
            }
            return null;
        }
    }
}