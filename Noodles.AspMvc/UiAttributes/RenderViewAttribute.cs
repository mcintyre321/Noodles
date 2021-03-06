﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class TransformViewAttribute : Attribute, ITransformHtml
    {
        public string ViewName { get; set; }

        public IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html)
        {
            return htmlHelper.Partial(ViewName, node);
        }
    }
}