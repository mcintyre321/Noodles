﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Noodles.AspMvc.Helpers
{
    using System;
    
    #line 2 "..\..\Helpers\NoodlesHelper.cshtml"
    using System.Collections;
    
    #line default
    #line hidden
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    
    #line 3 "..\..\Helpers\NoodlesHelper.cshtml"
    using System.Web.Mvc;
    
    #line default
    #line hidden
    using System.Web.Mvc.Ajax;
    
    #line 4 "..\..\Helpers\NoodlesHelper.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 10 "..\..\Helpers\NoodlesHelper.cshtml"
    using FormFactory;
    
    #line default
    #line hidden
    
    #line 5 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.AspMvc;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.AspMvc.Helpers;
    
    #line default
    #line hidden
    
    #line 8 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.Helpers;
    
    #line default
    #line hidden
    
    #line 11 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.Models;
    
    #line default
    #line hidden
    
    #line 12 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.RequestHandling;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.5.4.0")]
    public static class NoodlesHelper
    {

public static System.Web.WebPages.HelperResult NodeMethodsDropdown(Resource obj)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 14 "..\..\Helpers\NoodlesHelper.cshtml"
  
#line default
#line hidden


#line 14 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodsDropdown(obj, null));

#line default
#line hidden


#line 14 "..\..\Helpers\NoodlesHelper.cshtml"
                                  
#line default
#line hidden

});

                                  }


public static System.Web.WebPages.HelperResult NodeMethodsDropdown(Resource obj, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 16 "..\..\Helpers\NoodlesHelper.cshtml"
  
#line default
#line hidden


#line 16 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodsDropdown(obj, null, null));

#line default
#line hidden


#line 16 "..\..\Helpers\NoodlesHelper.cshtml"
                                        
#line default
#line hidden

});

                                        }


public static System.Web.WebPages.HelperResult NodeMethodsDropdown(Resource obj, string additionalClasses, string excludedMethodNamesCsv)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 18 "..\..\Helpers\NoodlesHelper.cshtml"
 
    var excludedMethodNames = (excludedMethodNamesCsv ?? "").Split(',');
    var filteredNodeMethods = obj.NodeMethods.ExceptNamed(excludedMethodNames);
    var filteredNodeLinks = obj.NodeLinks.ExceptNamed(excludedMethodNames);
    if (filteredNodeMethods.Cast<object>().Concat(filteredNodeLinks).Any())
    {
    
#line default
#line hidden


#line 24 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, BootstrapHelper.DropdownLinksButton("Actions",item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\r\n");



#line 25 "..\..\Helpers\NoodlesHelper.cshtml"
     foreach (var method in filteredNodeMethods)
    {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "        <li>");



#line 27 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, NodeMethodLink(method));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "</li>\r\n");



#line 28 "..\..\Helpers\NoodlesHelper.cshtml"
    }

#line default
#line hidden



#line 29 "..\..\Helpers\NoodlesHelper.cshtml"
     foreach (var link in filteredNodeLinks)
    {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "        <li><a href=\"");



#line 31 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, link.Url);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\">");



#line 31 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, link.DisplayName);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "</a></li>\r\n");



#line 32 "..\..\Helpers\NoodlesHelper.cshtml"
    }

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\r\n    ");



#line 34 "..\..\Helpers\NoodlesHelper.cshtml"
         }), additionalClasses));

#line default
#line hidden


#line 34 "..\..\Helpers\NoodlesHelper.cshtml"
                               
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 39 "..\..\Helpers\NoodlesHelper.cshtml"
  
#line default
#line hidden


#line 39 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodLink(method, null));

#line default
#line hidden


#line 39 "..\..\Helpers\NoodlesHelper.cshtml"
                               
#line default
#line hidden

});

                               }


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 41 "..\..\Helpers\NoodlesHelper.cshtml"
 
    
#line default
#line hidden


#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodLink(method, additionalClasses,item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");



#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
         WebViewPage.WriteTo(@__razor_template_writer, method.InvokeDisplayName);

#line default
#line hidden



#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                    })));

#line default
#line hidden


#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                       

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method, string additionalClasses, Func<dynamic, HelperResult> innerHtml)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 45 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "        <a class=\"nodeMethodLink ");



#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, additionalClasses ?? "");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, " ");



#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
                  WebViewPage.WriteTo(@__razor_helper_writer, method.Target.NodeType().Name.ToClassName());

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" href=\"");



#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                       WebViewPage.WriteTo(@__razor_helper_writer, method.InvokeUrl);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\">");



#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                          WebViewPage.WriteTo(@__razor_helper_writer, innerHtml(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "</a>\r\n");



#line 49 "..\..\Helpers\NoodlesHelper.cshtml"
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodForm(System.Web.Mvc.HtmlHelper html, IInvokeable method, string formClass)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 52 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
    
#line default
#line hidden


#line 55 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, html.Partial("Noodles/NodeMethod", method, new ViewDataDictionary { { "FormClass", formClass ?? "" } }));

#line default
#line hidden


#line 55 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                                            
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodForm(System.Web.Mvc.HtmlHelper html, IInvokeable method, string formClass, ViewDataDictionary dict)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 59 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
        dict = dict ?? new ViewDataDictionary();
        dict["FormClass"] = formClass;
    
#line default
#line hidden


#line 64 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodForm(html, method, dict));

#line default
#line hidden


#line 64 "..\..\Helpers\NoodlesHelper.cshtml"
                                       
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodForm(System.Web.Mvc.HtmlHelper html, IInvokeable method, ViewDataDictionary dict)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 68 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
        dict = dict ?? new ViewDataDictionary();

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <div class=\"nodeMethod\">\r\n        ");



#line 73 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, html.Partial("Noodles/NodeMethod", method, dict));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\r\n    </div>\r\n");



#line 75 "..\..\Helpers\NoodlesHelper.cshtml"
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodFormInline(System.Web.Mvc.HtmlHelper html, NodeMethod method)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 78 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
    
#line default
#line hidden


#line 81 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodFormInline(html, method, new ViewDataDictionary()));

#line default
#line hidden


#line 81 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                 
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodFormInline(System.Web.Mvc.HtmlHelper html, NodeMethod method, ViewDataDictionary viewData)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 85 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
        viewData = viewData ?? new ViewDataDictionary();
        viewData.Add("Inline", true);
    
#line default
#line hidden


#line 90 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodForm(html, method, "form-inline", viewData));

#line default
#line hidden


#line 90 "..\..\Helpers\NoodlesHelper.cshtml"
                                                          
    }

#line default
#line hidden

});

}


    }
}
#pragma warning restore 1591
