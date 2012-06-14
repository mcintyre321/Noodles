using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Noodles.Web
{
    public static class PathExtension
    {
        public static T SetUrlRoot<T>(this T root, ControllerContext cc)
        {
            UrlHelper u = new UrlHelper(cc.RequestContext);
            string url = u.Action(cc.RequestContext.RouteData.Values["action"] as string, cc.RequestContext.RouteData.Values["controller"] as string, new {path = ""});
            return root.SetUrlRoot(url);
        }
    

    }
}