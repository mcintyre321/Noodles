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

namespace WebNoodle.Example.Views.Shared.WebNoodle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    
    #line 1 "..\..\Views\Shared\WebNoodle\NodeActionsLink.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\Shared\WebNoodle\NodeActionsLink.cshtml"
    using WebNoodle;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/WebNoodle/NodeActionsLink.cshtml")]
    public class NodeActionsLink : System.Web.Mvc.WebViewPage<object>
    {
        public NodeActionsLink()
        {
        }
        public override void Execute()
        {




            
            #line 4 "..\..\Views\Shared\WebNoodle\NodeActionsLink.cshtml"
 if (Model.NodeMethods().Any())
{

            
            #line default
            #line hidden
WriteLiteral("    <a href=\"#loadactions\" class=\"nodeActionsLink\" data-nodepath=\"");


            
            #line 6 "..\..\Views\Shared\WebNoodle\NodeActionsLink.cshtml"
                                                             Write(Model.Path());

            
            #line default
            #line hidden
WriteLiteral("\" >(edit)</a>\r\n");


            
            #line 7 "..\..\Views\Shared\WebNoodle\NodeActionsLink.cshtml"
}

            
            #line default
            #line hidden

            
            #line 8 "..\..\Views\Shared\WebNoodle\NodeActionsLink.cshtml"
Write(Html.Partial("WebNoodle/NodeActionsLinkScript"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


        }
    }
}
#pragma warning restore 1591
