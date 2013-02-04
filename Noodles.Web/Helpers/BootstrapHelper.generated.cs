﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Noodles.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    
    #line 2 "..\..\Helpers\BootstrapHelper.cshtml"
    using System.Web.Mvc;
    
    #line default
    #line hidden
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.5.0.0")]
    public static class BootstrapHelper
    {

public static System.Web.WebPages.HelperResult DropdownLinksButton(string buttonTitle, Func<MvcHtmlString, HelperResult> links) {
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 3 "..\..\Helpers\BootstrapHelper.cshtml"
                                                                                           
#line default
#line hidden


#line 3 "..\..\Helpers\BootstrapHelper.cshtml"
                                                WebViewPage.WriteTo(@__razor_helper_writer, DropdownLinksButton(buttonTitle, links, null));

#line default
#line hidden


#line 3 "..\..\Helpers\BootstrapHelper.cshtml"
                                                                                                                                         
#line default
#line hidden

});

                                                                                                                                         }


public static System.Web.WebPages.HelperResult DropdownLinksButton(string buttonTitle, Func<MvcHtmlString, HelperResult> links, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 5 "..\..\Helpers\BootstrapHelper.cshtml"
 

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <div class=\"btn-group\">\r\n        <a class=\"btn dropdown-toggle ");



#line 7 "..\..\Helpers\BootstrapHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, additionalClasses ?? "");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" data-toggle=\"dropdown\" href=\"#\">");



#line 7 "..\..\Helpers\BootstrapHelper.cshtml"
                                                       WebViewPage.WriteTo(@__razor_helper_writer, buttonTitle);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, " <span class=\"caret\"></span></a>\r\n        <ul class=\"dropdown-menu\">\r\n           " +
" ");



#line 9 "..\..\Helpers\BootstrapHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, links(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\r\n        </ul>\r\n    </div>\r\n");



#line 12 "..\..\Helpers\BootstrapHelper.cshtml"

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult Modal(string header, Func<MvcHtmlString, HelperResult> body, Func<MvcHtmlString, HelperResult> footer)
    {
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 15 "..\..\Helpers\BootstrapHelper.cshtml"
     

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <div class=\"modal\">\r\n        <div class=\"modal-header\">\r\n            <a class" +
"=\"close\" data-dismiss=\"modal\">×</a>\r\n            <h3>");



#line 19 "..\..\Helpers\BootstrapHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, header);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "</h3>\r\n        </div>\r\n        <div class=\"modal-body\">\r\n            ");



#line 22 "..\..\Helpers\BootstrapHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, body(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\r\n        </div>\r\n        <div class=\"modal-footer\">\r\n            ");



#line 25 "..\..\Helpers\BootstrapHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, footer(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\r\n        </div>\r\n    </div>\r\n");



#line 28 "..\..\Helpers\BootstrapHelper.cshtml"

#line default
#line hidden

});

}


    }
}
#pragma warning restore 1591
