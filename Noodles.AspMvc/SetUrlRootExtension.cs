using System.Web.Mvc;

namespace Noodles.AspMvc
{
    public static class SetUrlRootExtension
    {
        public static T SetUrlRoot<T>(this T root, ControllerContext cc)
        {
            var u = new UrlHelper(cc.RequestContext);
            string url = u.Action(cc.RequestContext.RouteData.Values["action"] as string, cc.RequestContext.RouteData.Values["controller"] as string, new {path = ""});
            return root.SetUrlRoot(url);
        }
    }
}