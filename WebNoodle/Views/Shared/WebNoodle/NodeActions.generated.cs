﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.488
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebNoodle.Views.Shared.WebNoodle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    
    #line 4 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
    using System.Linq;
    
    #line default
    #line hidden
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    
    #line 1 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
    using FormFactory;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
    using WebNoodle;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/WebNoodle/NodeActions.cshtml")]
    public class NodeActions : System.Web.Mvc.WebViewPage<object>
    {
        public NodeActions()
        {
        }
        public override void Execute()
        {






            
            #line 6 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
 if (Model.NodeMethods().Any())
{

            
            #line default
            #line hidden
WriteLiteral("    <div id=\"actions-");


            
            #line 8 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
                Write(Model.Id());

            
            #line default
            #line hidden
WriteLiteral("\">\r\n        <ul>\r\n");


            
            #line 10 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
             foreach (var nodeMethod in Model.NodeMethods())
            {

            
            #line default
            #line hidden
WriteLiteral("                <li>");


            
            #line 12 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
               Write(Html.Partial("WebNoodle/NodeAction", nodeMethod));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");


            
            #line 13 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </ul>\r\n    </div>\r\n");


            
            #line 16 "..\..\Views\Shared\WebNoodle\NodeActions.cshtml"
}

            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
