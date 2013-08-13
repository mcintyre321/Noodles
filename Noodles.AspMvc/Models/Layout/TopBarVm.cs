﻿using System.Collections.Generic;

namespace Noodles.AspMvc.Models.Layout
{
    public class TopBarVm
    {
        public TopBarVm()
        {
            Brand = new BrandVm();
            LeftItems = new List<NavItemVm>();
            RightItems = new List<NavItemVm>();
            AccountMenu = new List<NavItemVm>();
        }

        public List<NavItemVm> RightItems { get; set; }
        public List<NavItemVm> AccountMenu { get; set; }
        public BrandVm Brand { get; set; }
        public List<NavItemVm> LeftItems { get; set; }

        public bool Fixed { get; set; }
    }
}