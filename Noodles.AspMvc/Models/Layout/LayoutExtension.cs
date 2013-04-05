using System.Web;
using System.Web.Mvc;

namespace Noodles.AspMvc.Models.Layout
{
    public static class LayoutExtension
    {
        public static LayoutVm LayoutVm(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.LayoutVm();
        }

        public static LayoutVm LayoutVm(this ControllerContext cc)
        {
            return cc.HttpContext.LayoutVm();
        }

        public static LayoutVm LayoutVm(this HttpContextBase context)
        {
            var vd = context.Items;
            var layout = vd[typeof(LayoutVm).FullName] as LayoutVm;
            if (layout == null)
            {
                layout = new LayoutVm();
                vd[typeof(LayoutVm).FullName] = layout;
            }
            return layout;
        }

    }
}