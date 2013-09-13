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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
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
  
#line default
#line hidden



#line 31 "..\..\Helpers\NoodlesHelper.cshtml"
  

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeLink(INode node)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 34 "..\..\Helpers\NoodlesHelper.cshtml"
  
#line default
#line hidden


#line 34 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeLink(node, null));

#line default
#line hidden


#line 34 "..\..\Helpers\NoodlesHelper.cshtml"
                       
#line default
#line hidden

});

                       }


public static System.Web.WebPages.HelperResult NodeLink(INode node, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 36 "..\..\Helpers\NoodlesHelper.cshtml"
 
    
#line default
#line hidden


#line 37 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeLink(node, additionalClasses,item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");



#line 37 "..\..\Helpers\NoodlesHelper.cshtml"
 WebViewPage.WriteTo(@__razor_template_writer, node.DisplayName);

#line default
#line hidden



#line 37 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                    })));

#line default
#line hidden


#line 37 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                       

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeLink(INode node, string additionalClasses, Func<dynamic, HelperResult> innerHtml)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 40 "..\..\Helpers\NoodlesHelper.cshtml"
 

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <a class=\"node-link ");



#line 41 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, additionalClasses ?? "");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, " ");



#line 41 "..\..\Helpers\NoodlesHelper.cshtml"
         WebViewPage.WriteTo(@__razor_helper_writer, node.DisplayName);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" href=\"");



#line 41 "..\..\Helpers\NoodlesHelper.cshtml"
                                   WebViewPage.WriteTo(@__razor_helper_writer, node.Url);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\">");



#line 41 "..\..\Helpers\NoodlesHelper.cshtml"
                                              WebViewPage.WriteTo(@__razor_helper_writer, innerHtml(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "</a>\r\n");



#line 42 "..\..\Helpers\NoodlesHelper.cshtml"

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 45 "..\..\Helpers\NoodlesHelper.cshtml"
  
#line default
#line hidden


#line 45 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodLink(method, null));

#line default
#line hidden


#line 45 "..\..\Helpers\NoodlesHelper.cshtml"
                               
#line default
#line hidden

});

                               }


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 47 "..\..\Helpers\NoodlesHelper.cshtml"
 
    
#line default
#line hidden


#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodLink(method, additionalClasses,item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");



#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
         WebViewPage.WriteTo(@__razor_template_writer, method.InvokeDisplayName);

#line default
#line hidden



#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                    })));

#line default
#line hidden


#line 48 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                       

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method, string additionalClasses, Func<dynamic, HelperResult> innerHtml)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 51 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <a class=\"nodeMethodLink ");



#line 54 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, additionalClasses ?? "");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, " ");



#line 54 "..\..\Helpers\NoodlesHelper.cshtml"
              WebViewPage.WriteTo(@__razor_helper_writer, method.Target.NodeType().Name.ToClassName());

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" href=\"");



#line 54 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                   WebViewPage.WriteTo(@__razor_helper_writer, method.InvokeUrl);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\">");



#line 54 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                      WebViewPage.WriteTo(@__razor_helper_writer, innerHtml(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "</a>\r\n");



#line 55 "..\..\Helpers\NoodlesHelper.cshtml"
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodForm(System.Web.Mvc.HtmlHelper html, IInvokeable method, string formClass)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 58 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
    
#line default
#line hidden


#line 61 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, html.Partial("Noodles/NodeMethod", method, new ViewDataDictionary { { "FormClass", formClass ?? "" } }));

#line default
#line hidden


#line 61 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                                            
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodForm(System.Web.Mvc.HtmlHelper html, IInvokeable method, string formClass, ViewDataDictionary dict)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 65 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
        dict = dict ?? new ViewDataDictionary();
        dict["FormClass"] = formClass;
    
#line default
#line hidden


#line 70 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodForm(html, method, dict));

#line default
#line hidden


#line 70 "..\..\Helpers\NoodlesHelper.cshtml"
                                       
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodForm(System.Web.Mvc.HtmlHelper html, IInvokeable method, ViewDataDictionary dict)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 74 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
        dict = dict ?? new ViewDataDictionary();

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <div class=\"nodeMethod\">\r\n        ");



#line 79 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, html.Partial("Noodles/NodeMethod", method, dict));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\r\n    </div>\r\n");



#line 81 "..\..\Helpers\NoodlesHelper.cshtml"
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodFormInline(System.Web.Mvc.HtmlHelper html, NodeMethod method)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 84 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
    
#line default
#line hidden


#line 87 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodFormInline(html, method, new ViewDataDictionary()));

#line default
#line hidden


#line 87 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                 
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodFormInline(System.Web.Mvc.HtmlHelper html, NodeMethod method, ViewDataDictionary viewData)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 91 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {
        viewData = viewData ?? new ViewDataDictionary();
        viewData.Add("Inline", true);
    
#line default
#line hidden


#line 96 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodForm(html, method, "form-inline", viewData));

#line default
#line hidden


#line 96 "..\..\Helpers\NoodlesHelper.cshtml"
                                                          
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult Form(HtmlHelper html, INode node)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 101 "..\..\Helpers\NoodlesHelper.cshtml"
 
    var settableProperties = ((IInvokeable)node).Parameters.Where(p => p.Readonly == false);
    
#line default
#line hidden


#line 103 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, Form(html, node, settableProperties));

#line default
#line hidden


#line 103 "..\..\Helpers\NoodlesHelper.cshtml"
                                         

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult Form(HtmlHelper html, INode node, IEnumerable<IInvokeableParameter> fieldsToDisplay)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 107 "..\..\Helpers\NoodlesHelper.cshtml"
 

    var isNodeMethod = node is NodeMethod;
    var fields = fieldsToDisplay as IInvokeableParameter[] ?? fieldsToDisplay.ToArray();
    if (isNodeMethod || fields.Any())
    {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "        <form class=\"node-form\" action=\"");



#line 113 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, node.Url);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" method=\"POST\">\r\n");



#line 114 "..\..\Helpers\NoodlesHelper.cshtml"
             if (html.ViewData.ModelState.SelectMany(ms => ms.Value.Errors).Any())
            {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "                <div class=\"noodles-callout noodles-callout-danger\">\r\n           " +
"         Please correct the issues below:\r\n                    ");



#line 118 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, html.ValidationSummary(true));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\r\n                </div>\r\n");



#line 120 "..\..\Helpers\NoodlesHelper.cshtml"
            }

#line default
#line hidden



#line 121 "..\..\Helpers\NoodlesHelper.cshtml"
             foreach (var field in fields)
            {
                var parameter = field;
                var vm = parameter.ToPropertyVm();
                
#line default
#line hidden


#line 125 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, html.Partial("FormFactory/Form.Property", vm));

#line default
#line hidden


#line 125 "..\..\Helpers\NoodlesHelper.cshtml"
                                                              
            }

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "            <input type=\"submit\" value=\"");



#line 127 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, isNodeMethod ? node.DisplayName : "Update");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" />\r\n\r\n        </form>\r\n");



#line 130 "..\..\Helpers\NoodlesHelper.cshtml"

    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult Property(HtmlHelper html, NodeProperty property)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 136 "..\..\Helpers\NoodlesHelper.cshtml"
 
    var vm = property.ToPropertyVm();
    vm.Readonly = true;
    html.RenderPartial("FormFactory/Form.Property", vm);

#line default
#line hidden

});

}


    }
}
#pragma warning restore 1591
