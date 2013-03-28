﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public static class UiHelpers
    {
        public static IEnumerable<NodeMethod> Hinted(this IEnumerable<NodeMethod> nodeMethods, string hint)
        {
            return nodeMethods.Where(nm => nm.UiHint == hint);
        }

        public static IEnumerable<NodeLink> Hinted(this IEnumerable<NodeLink> links, string hint)
        {
            return links.Where(nm => nm.UiHint == hint);
        } 

    }
}