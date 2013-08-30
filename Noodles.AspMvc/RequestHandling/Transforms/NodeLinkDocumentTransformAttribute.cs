using System;
using System.Web.Mvc;
using CsQuery;
using Noodles.AspMvc.Infrastructure;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public abstract class ChildNodeDocumentTransformAttribute : Attribute
    {
        public void Transform(INode child, ControllerContext cc, INode parent)
        {
            cc.HttpContext.Items.AddTransform(cq =>
            {
                var element = cq["[data-node-url='" + child.Url + "']"];
                Transform(element, child, cc, parent);
            });
        }

        public abstract void Transform(CQ elements, INode child, ControllerContext cc, INode parent);
    }
}