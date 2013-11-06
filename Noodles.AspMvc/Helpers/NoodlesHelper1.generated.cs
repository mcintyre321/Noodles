﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Noodles.AspMvc.Helpers
{
    using System;
    using System.Collections.Generic;
    
    #line 2 "..\..\Helpers\NoodlesHelper.cshtml"
    using System.ComponentModel;
    
    #line default
    #line hidden
    using System.IO;
    using System.Linq;
    using System.Net;
    
    #line 3 "..\..\Helpers\NoodlesHelper.cshtml"
    using System.Reflection;
    
    #line default
    #line hidden
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    
    #line 4 "..\..\Helpers\NoodlesHelper.cshtml"
    using System.Web.Mvc;
    
    #line default
    #line hidden
    using System.Web.Mvc.Ajax;
    
    #line 5 "..\..\Helpers\NoodlesHelper.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 6 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.AspMvc;
    
    #line default
    #line hidden
    
    #line 8 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.AspMvc.Helpers;
    
    #line default
    #line hidden
    
    #line 9 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.AspMvc.UiAttributes;
    
    #line default
    #line hidden
    
    #line 10 "..\..\Helpers\NoodlesHelper.cshtml"
    using Noodles.Models;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    public static class NoodlesHelper
    {

public static System.Web.WebPages.HelperResult NodeLink(HtmlHelper html, INode node)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 13 "..\..\Helpers\NoodlesHelper.cshtml"
  
#line default
#line hidden


#line 13 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeLink(html, node, null));

#line default
#line hidden


#line 13 "..\..\Helpers\NoodlesHelper.cshtml"
                             
#line default
#line hidden

});

                             }


public static System.Web.WebPages.HelperResult NodeLink(HtmlHelper html, INode node, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 15 "..\..\Helpers\NoodlesHelper.cshtml"
 
    
#line default
#line hidden


#line 16 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeLink(html, node, additionalClasses,item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");



#line 16 "..\..\Helpers\NoodlesHelper.cshtml"
       WebViewPage.WriteTo(@__razor_template_writer, node.DisplayName);

#line default
#line hidden



#line 16 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                          })));

#line default
#line hidden


#line 16 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                             

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeLink(HtmlHelper htmlHelper, INode node, string additionalClasses, Func<dynamic, HelperResult> innerHtml)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 19 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (node != null)
    {
        Func<dynamic, HelperResult> initHtml = (item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, "<a class=\"node-link ");



#line 22 "..\..\Helpers\NoodlesHelper.cshtml"
                         WebViewPage.WriteTo(@__razor_template_writer, additionalClasses ?? "");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");



#line 22 "..\..\Helpers\NoodlesHelper.cshtml"
                                                    WebViewPage.WriteTo(@__razor_template_writer, node.Name);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\" href=\"");



#line 22 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                       WebViewPage.WriteTo(@__razor_template_writer, node.Url);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\">");



#line 22 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                  WebViewPage.WriteTo(@__razor_template_writer, innerHtml(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "</a>");



#line 22 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                                                                                 }));
        var html = initHtml(null).ToHtmlString();
        foreach (var transformAtt in node.Attributes.OfType<ITransformHtml>())
        {
            html = transformAtt.Transform(htmlHelper, node, html).ToHtmlString();
        }
    
#line default
#line hidden


#line 28 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, htmlHelper.Raw(html));

#line default
#line hidden


#line 28 "..\..\Helpers\NoodlesHelper.cshtml"
                         
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 33 "..\..\Helpers\NoodlesHelper.cshtml"
  
#line default
#line hidden


#line 33 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodLink(method, null));

#line default
#line hidden


#line 33 "..\..\Helpers\NoodlesHelper.cshtml"
                               
#line default
#line hidden

});

                               }


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 35 "..\..\Helpers\NoodlesHelper.cshtml"
 
    
#line default
#line hidden


#line 36 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, NodeMethodLink(method, additionalClasses,item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");



#line 36 "..\..\Helpers\NoodlesHelper.cshtml"
         WebViewPage.WriteTo(@__razor_template_writer, method.InvokeDisplayName);

#line default
#line hidden



#line 36 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                    })));

#line default
#line hidden


#line 36 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                       

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult NodeMethodLink(IInvokeable method, string additionalClasses, Func<dynamic, HelperResult> innerHtml)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 39 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (method != null)
    {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <a class=\"nodeMethodLink ");



#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, additionalClasses ?? "");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, " ");



#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
              WebViewPage.WriteTo(@__razor_helper_writer, method.Target.NodeType().Name.ToClassName());

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\" href=\"");



#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                   WebViewPage.WriteTo(@__razor_helper_writer, method.InvokeUrl);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\">");



#line 42 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                                      WebViewPage.WriteTo(@__razor_helper_writer, innerHtml(null));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_helper_writer, "</a>\r\n");



#line 43 "..\..\Helpers\NoodlesHelper.cshtml"
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult Form(HtmlHelper html, INode node)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 47 "..\..\Helpers\NoodlesHelper.cshtml"
 
    var settableProperties = ((IInvokeable)node).Parameters.Where(p => p.Readonly == false);
    
#line default
#line hidden


#line 49 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, Form(html, node, settableProperties));

#line default
#line hidden


#line 49 "..\..\Helpers\NoodlesHelper.cshtml"
                                         

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult Form(HtmlHelper htmlHelper, INode node, IEnumerable<IInvokeableParameter> fieldsToDisplay)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 53 "..\..\Helpers\NoodlesHelper.cshtml"
 
    if (node != null)
    {
        Func<dynamic, HelperResult> initHtml = (item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, "\r\n");



#line 57 "..\..\Helpers\NoodlesHelper.cshtml"
      
        var isNodeMethod = node is NodeMethod;
        var fields = fieldsToDisplay as IInvokeableParameter[] ?? fieldsToDisplay.ToArray();
        if (isNodeMethod || fields.Any())
        {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "        <form class=\"node-form\" action=\"");



#line 62 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, node.Url);

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\" method=\"POST\">\r\n");



#line 63 "..\..\Helpers\NoodlesHelper.cshtml"
              
            var descriptionAttribute = node.Attributes.OfType<DescriptionAttribute>().SingleOrDefault();
            if (descriptionAttribute != null)
            {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "                <div class=\"noodles-callout noodles-callout-info\">\r\n             " +
"       ");



#line 68 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, htmlHelper.Raw(descriptionAttribute.Description));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\r\n                </div>\r\n");



#line 70 "..\..\Helpers\NoodlesHelper.cshtml"
            }
            

#line default
#line hidden



#line 72 "..\..\Helpers\NoodlesHelper.cshtml"
             if (htmlHelper.ViewData.ModelState.SelectMany(ms => ms.Value.Errors).Any())
            {

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "                <div class=\"noodles-callout noodles-callout-danger\">\r\n           " +
"         Please correct the issues below: ");



#line 75 "..\..\Helpers\NoodlesHelper.cshtml"
        WebViewPage.WriteTo(@__razor_template_writer, htmlHelper.ValidationSummary(true));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\r\n                </div>\r\n");



#line 77 "..\..\Helpers\NoodlesHelper.cshtml"
            }

#line default
#line hidden



#line 78 "..\..\Helpers\NoodlesHelper.cshtml"
             foreach (var field in fields)
            {
                var parameter = field;
                var vm = parameter.ToPropertyVm();
                
#line default
#line hidden


#line 82 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, htmlHelper.Partial("FormFactory/Form.Property", vm));

#line default
#line hidden


#line 82 "..\..\Helpers\NoodlesHelper.cshtml"
                                                                    
            }

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "            <input type=\"submit\" value=\"");



#line 84 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, isNodeMethod ? node.DisplayName : "Update");

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\" />\r\n        </form>\r\n");



#line 86 "..\..\Helpers\NoodlesHelper.cshtml"
        }

    

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "    ");



#line 89 "..\..\Helpers\NoodlesHelper.cshtml"
         }));
        var html = initHtml(null).ToHtmlString();
        foreach (var transformAtt in node.Attributes.OfType<ITransformHtml>().Where(a => a.GetType().GetCustomAttribute<NotInFormHelperAttribute>() == null))
        {
            html = transformAtt.Transform(htmlHelper, node, html).ToHtmlString();
        }
    
#line default
#line hidden


#line 95 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, htmlHelper.Raw(html));

#line default
#line hidden


#line 95 "..\..\Helpers\NoodlesHelper.cshtml"
                         
    }

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult DropdownNode(HtmlHelper Html, INode node, string additionalClasses)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 100 "..\..\Helpers\NoodlesHelper.cshtml"
 
    
#line default
#line hidden


#line 101 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_helper_writer, BootstrapHelper.DropdownLinksButton(node.DisplayName,item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

#line default
#line hidden


WebViewPage.WriteLiteralTo(@__razor_template_writer, " ");

WebViewPage.WriteLiteralTo(@__razor_template_writer, "<li style=\"margin: 20px; width: 400px;\" class=\"keep-open\">\r\n        ");



#line 102 "..\..\Helpers\NoodlesHelper.cshtml"
WebViewPage.WriteTo(@__razor_template_writer, NoodlesHelper.Form(Html, node));

#line default
#line hidden

WebViewPage.WriteLiteralTo(@__razor_template_writer, "\r\n    </li>");



#line 103 "..\..\Helpers\NoodlesHelper.cshtml"
       }), additionalClasses));

#line default
#line hidden


#line 103 "..\..\Helpers\NoodlesHelper.cshtml"
                             

#line default
#line hidden

});

}


public static System.Web.WebPages.HelperResult Property(HtmlHelper html, NodeProperty property)
{
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {



#line 107 "..\..\Helpers\NoodlesHelper.cshtml"
 
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
