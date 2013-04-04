using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
            Footer = new FooterVm();
        }

        public TopBarVm TopBar { get; set; }
        public FooterVm Footer { get; set; }

        public bool IsFluid { get; set; }
    }

    public class FooterVm
    {
        public FooterVm()
        {
            HtmlColumns = new List<string>();
        }

        public IList<string> HtmlColumns { get; set; }
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

        public bool Fixed { get; set; }
    }

    public class NavItemVm
    {
        public NavItemVm()
        {
            Html = "";
        }
        public string Html { get; set; }
        public bool Active { get; set; }
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