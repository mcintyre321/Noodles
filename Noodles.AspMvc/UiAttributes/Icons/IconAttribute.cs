using System;
using System.Web.Mvc;
using CsQuery;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes.Icons
{
    public class IconAttribute : Noodles.AspMvc.RequestHandling.Transforms.ChildNodeDocumentTransformAttribute
    {
        public string IconName { get; private set; }

        public IconAttribute(string iconName)
        {
            IconName = iconName;
        }

        public override void Transform(CQ elements, INode child, ControllerContext cc, INode parent)
        {
            var link = elements.Find("> a");
            link.Html("<i class=\"icon-" + IconName + "\"></i>&nbsp;" + link.Html());
        }
    }
}