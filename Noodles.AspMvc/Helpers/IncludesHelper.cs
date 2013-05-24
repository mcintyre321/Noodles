using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Noodles.AspMvc.Models.Layout;

namespace Noodles.AspMvc.Helpers
{
    public static class IncludesHelper
    {
        public static void RegisterScripts(this LayoutVm layout)
        {
            layout.HeadBundle.ScriptIncludes.Add("/Scripts/modernizr-2.6.2.js");
            var styles = layout.HeadBundle.StyleIncludes;
            styles.Add("/Content/bootstrap.min.css");
            styles.Add("/Content/bootstrap-overrides.css");
            styles.Add("/Content/font-awesome.min.css");
            styles.Add("/Content/themes/base/jquery-ui.css");
            styles.Add("/Content/Noodles.AspMvc.css");

            var scripts = layout.HeadBundle.ScriptIncludes;
            scripts.Add("/Scripts/jquery-1.9.1.min.js");
            scripts.Add("/Scripts/jquery-migrate-1.1.1.js");
            scripts.Add("/Scripts/jquery-ui-1.10.2.min.js");
            scripts.Add("/Scripts/bootstrap.js");
            scripts.Add("/Scripts/Noodles/Noodles.js");
            scripts.Add("/Scripts/jquery.validate.js");
            scripts.Add("/Scripts/jquery.validate.unobtrusive.js");
            scripts.Add("/Scripts/jquery.unobtrusive-ajax.js");
            scripts.Add("/Scripts/formfactory/formfactory.js");
            scripts.Add("/Scripts/jquery.validate.unobtrusive.dynamic.js");
            scripts.Add("/Scripts/jquery.signalR-1.0.0-rc2.js");
        }
    }
}