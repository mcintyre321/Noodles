using System;
using System.Web.Mvc;
using CsQuery;
using Noodles.AspMvc.Infrastructure;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public abstract class NodeLinkDocumentTransformAttribute : Attribute
    {
        public void Transform(NodeLink nodeLink, ControllerContext cc, INode node)
        {
            cc.HttpContext.Items.AddTransform(cq =>
            {
                var element = cq["[data-node-url='" + nodeLink.Url + "']"];
                Transform(element, nodeLink, cc, node);
            });
        }

        public abstract void Transform(CQ element, NodeLink nodeLink, ControllerContext cc, INode resource);
    }
}