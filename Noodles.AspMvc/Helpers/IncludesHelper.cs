using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsQuery;
using Noodles.AspMvc.Infrastructure;
using Noodles.AspMvc.Models.Layout;

namespace Noodles.AspMvc.Helpers
{
    public static class IncludesHelper
    {
        public static void RegisterScripts(this IDictionary contextItems)
        {
            
            var styles = new[]
            {
                "/Content/bootstrap/bootstrap.min.css",
                "/Content/bootstrap-overrides.css",
                "/Content/font-awesome.min.css",
                "/Content/themes/base/jquery-ui.css",
                "/Content/Noodles.AspMvc.css",
                "/Content/formfactory/formfactory.css",
            };
            
            var scripts = new[]
            {
                "/Scripts/modernizr-2.6.2.js",
                "/Scripts/jquery-1.9.1.min.js",
                "/Scripts/jquery-migrate-1.1.1.js",
                "/Scripts/jquery-ui-1.10.2.min.js",
                "/Scripts/bootstrap.js",
                "/Scripts/bootstrap3-typeahead.js",
                "/Scripts/bootstrap3-typeahead.unobtrusive.js",
                "/Scripts/jquery.validate.js",
                "/Scripts/jquery.validate.unobtrusive.js",
                "/Scripts/jquery.unobtrusive-ajax.js",
                "/Scripts/formfactory/formfactory.js",
                "/Scripts/jquery.validate.unobtrusive.dynamic.js",
                "/Scripts/jquery.signalR-1.0.0-rc2.js",
                "/Scripts/Noodles/Noodles.js"
            };

            contextItems.AddDocTransform(cq =>
            {
                var head = cq.Find("head");
                foreach (var item in styles)
                {
                    head.Append(CQ.Create("<link rel='stylesheet' href='" + item + "' />"));
                }
                foreach (var item in scripts)
                {
                    head.Append(CQ.Create("<script type='text/javascript' src='" + item + "' ></script>"));
                }

            });
            
        }
    }
}