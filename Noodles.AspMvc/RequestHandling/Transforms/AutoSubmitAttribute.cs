using System;
using System.Web.Mvc;
using CsQuery;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoSubmitAttribute : ChildNodeDocumentTransformAttribute
    {
        public bool AutoSubmit { get; private set; }

        public AutoSubmitAttribute(bool autoSubmit = true)
        {
            AutoSubmit = autoSubmit;
        }

        public override void Transform(CQ element, INode child, ControllerContext cc, INode parent)
        {
            var link = element.Find("> a");
            link.AddClass("auto-submit");

        }
    }
}