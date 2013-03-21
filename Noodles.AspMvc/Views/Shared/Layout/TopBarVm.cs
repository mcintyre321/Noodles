using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Noodles.AspMvc.Views.Shared.Layout
{
    
    public static class LayoutExtension
    {
        public static LayoutVm LayoutVm(this HtmlHelper helper)
        {
            var vd = helper.ViewContext.HttpContext.Items;
            var layout = vd[typeof (LayoutVm).FullName] as LayoutVm;
            if (layout == null)
            {
                layout = new LayoutVm();
                vd[typeof (LayoutVm).FullName] = layout;
            }
            return layout;
        }
    }

    public class LayoutVm
    {
        public LayoutVm()
        {
            TopBar = new TopBarVm();
        }
        public TopBarVm TopBar { get; set; }
    }

    public class TopBarVm
    {
        public TopBarVm()
        {
            Brand = new BrandVm();
            LeftItems = new List<NavItemVm>();
            RightItems = new List<NavItemVm>();

        }

        public List<NavItemVm> RightItems { get; set; }
        public BrandVm Brand { get; set; }
        public List<NavItemVm> LeftItems { get; set; }
    }

    public class NavItemVm
    {
        public NavItemVm()
        {
            Html = "";
            Href = "";
            Class = "";
            Children = new List<NavItemVm>();
        }
        public string Html { get; set; }
        public string Href { get; set; }
        public string Class { get; set; }
        public bool Active { get; set; }
        public IList<NavItemVm> Children { get; set; }
    }

    public class BrandVm
    {
        public BrandVm()
        {
            Html = "Application";
        }
        public string Html { get; set; }
        public string Href { get; set; }
    }
}