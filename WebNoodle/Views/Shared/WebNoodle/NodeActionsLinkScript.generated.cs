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
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.3.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/WebNoodle/NodeActionsLinkScript.cshtml")]
    public class NodeActionsLinkScript : System.Web.Mvc.WebViewPage<dynamic>
    {
        public NodeActionsLinkScript()
        {
        }
        public override void Execute()
        {

            
            #line 1 "..\..\Views\Shared\WebNoodle\NodeActionsLinkScript.cshtml"
 if (HttpContext.Current.Items["NeedToWriteNodeActionsLinkScript"] as bool? ?? true)
{
    HttpContext.Current.Items["NeedToWriteNodeActionsLinkScript"] = false;


            
            #line default
            #line hidden
WriteLiteral("    <script type=\"text/javascript\">\r\n        $(\".popover .close\").live(\'click\', f" +
"unction (e) { $(this).closest(\".popover\").hide(); });\r\n        $(\".nodeActionsLi" +
"nk\").live(\'click\', function (e) {\r\n            var $link = $(this);\r\n           " +
" if (e.target != this) return false;\r\n            var nodePath = $link.attr(\"dat" +
"a-nodepath\");\r\n            var nodeId = $link.attr(\"data-nodeid\");\r\n            " +
"if ($(\'#actions-\' + nodeId).length) {\r\n                return false;\r\n          " +
"  }\r\n            if ($link.attr(\"data-content\") === undefined) {\r\n              " +
"  $link.attr(\"data-content\", \"\");\r\n                var actionUrl = nodePath + \"?" +
"action=getNodeActions\";\r\n                $.get(actionUrl, {}, function (data) {\r" +
"\n                    $link.attr(\"data-content\", data)\r\n                        ." +
"popover({\r\n                            trigger: \"manual\",\r\n                     " +
"       placement: \"right\",\r\n                            offset: 0,\r\n            " +
"                html: true,\r\n                            delayOut: 500,\r\n       " +
"                     title: function () { return \"Actions<a class=\'close\' href=\'" +
"#\'>×</a>\"; }\r\n\r\n                        })\r\n                        .click(funct" +
"ion () {\r\n                            $(\".popover\").hide();\r\n                   " +
"         $link.popover(\'show\');\r\n\r\n                            console.log(\"show" +
"ing\");\r\n                            var actionName = $link.attr(\"data-actionname" +
"\");\r\n                            if (actionName) {\r\n                            " +
"    $(\"#\" + nodeId + \"_\" + actionName + \"_actionlink\").click();\r\n\r\n             " +
"               }\r\n                        })\r\n                        .click();\r" +
"\n\r\n                });\r\n            }\r\n            return false;\r\n        });\r\n " +
"       $(\".nodeActionsPanelLink\").live(\'click\', function (e) {\r\n            var " +
"panelId = $(this).attr(\"id\") + \"panel\";\r\n            $(\"#\" + panelId).modal({ sh" +
"ow: true, backdrop: true });\r\n            $(\"#\" + panelId + \" :input:visible:ena" +
"bled:first\").focus();\r\n            e.stopPropagation();\r\n        });\r\n        $(" +
"\".post-via-ajax\").live(\'click\', function (e) {\r\n            var $form = $(this)." +
"closest(\"form\");\r\n            $.ajax({\r\n                url: $form.attr(\'action\'" +
"),\r\n                type: \"POST\",\r\n                data: $form.serialize(),\r\n   " +
"             success: function (data) {\r\n                    if (data === \"OK\") " +
"{\r\n                        window.location.reload();\r\n                    } else" +
" {\r\n                        $form.parent().html(data);\r\n                    }\r\n " +
"               },\r\n                error: function (jqXhr, textStatus, errorThro" +
"wn) {\r\n\r\n                    console.log(\"Error \'\" + jqXhr.status + \"\' (textStat" +
"us: \'\" + textStatus + \"\', errorThrown: \'\" + errorThrown + \"\')\");\r\n              " +
"  },\r\n                complete: function () {\r\n                    //$(\"#Progres" +
"sDialog\").dialog(\"close\");\r\n                }\r\n            });\r\n            retu" +
"rn false;\r\n        });\r\n    </script>\r\n");


            
            #line 76 "..\..\Views\Shared\WebNoodle\NodeActionsLinkScript.cshtml"
}
            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
