using System;
using System.Web.Mvc;
using Noodles.AspMvc.Infrastructure;
using Noodles.AspMvc.RequestHandling.Transforms;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    public class NoContainerAttribute : Attribute
    {
    }
    public class RemoveFromViewAttribute : Attribute, ITransformContextUsingChildNode
    {
        public void Transform(ControllerContext cc, INode child)
        {
            cc.HttpContext.Items.AddDocTransform(doc => doc[".node-actions-container > a[href='" + child.Url + "']"].Remove());

        }
    }
}