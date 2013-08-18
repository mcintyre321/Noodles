using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsQuery;
using Noodles.AspMvc.Infrastructure;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class TransformRuleRegistry
    {
        public List<IDocumentTransform> Transforms { get; private set; }
        public static TransformRuleRegistry Default { get; private set; }
        static TransformRuleRegistry()
        {
            Default = new TransformRuleRegistry()
            {
                Transforms =
                {
                    new ApplyAttributeTransforms(),
                    new SelectFragmentTransform()
                }
            };
        }

        public TransformRuleRegistry()
        {
            Transforms = new List<IDocumentTransform>();
        }

        public void RegisterTransformations(ControllerContext cc, INode resource)
        {
            foreach (var documentTransform in Transforms)
            {
                documentTransform.Register(cc, resource);
            }
        }

    }

    public class SelectFragmentTransform : IDocumentTransform
    {
        public void Register(ControllerContext cc, INode resource)
        {
            var fragmentSelector = cc.RequestContext.HttpContext.Request["fragment-selector"];
            if (fragmentSelector != null)
            {
                cc.HttpContext.Items.AddTransform(cq => CQ.CreateFragment(cq.Document.QuerySelector(fragmentSelector).OuterHTML));
            }
        }
    }

    public class ApplyAttributeTransforms : IDocumentTransform
    {
        public void Register(ControllerContext cc, INode resource)
        {
            foreach (var nodeLink in resource.ChildNodes.OfType<NodeLink>())
            {
                foreach (var att in nodeLink.Attributes.OfType<NodeLinkDocumentTransformAttribute>())
                {
                    att.Transform(nodeLink, cc, resource);
                }
            }
        }
    }

    public interface IDocumentTransform
    {
        void Register(ControllerContext cc, INode resource);
    }

    public class ModalAttribute: NodeLinkDocumentTransformAttribute
    {
        public override void Transform(CQ element, NodeLink nodeLink, ControllerContext cc, INode resource)
        {
            var link = element.Find("> a");
            var uri = link.Attr("href");
            if (!uri.Contains("?")) uri += "?";
            uri += "fragment-selector=.node-container";
            link.Attr("href", uri);
            link.Data("toggle", "modal").Data("target", "#noodles-modal");
            
            if (element.Document.QuerySelector("#noodles-modal") == null)
            {
                CQ.Create("<div class=\"modal hide\" id=\"noodles-modal\"><div class=\"modal-header\"><a class=\"close\" data-dismiss=\"modal\">×</a><h3 class=\"title\">&nbsp;</h3></div><div class=\"modal-body\"></div><div class=\"modal-footer\"></div></div>")
                    .AppendTo(element.Document.Body);

            }
        }
    }

    public abstract class NodeLinkDocumentTransformAttribute : Attribute
    {
        public void Transform(NodeLink nodeLink, ControllerContext cc, INode resource)
        {
             cc.HttpContext.Items.AddTransform(cq =>
             {
                 var element = cq["[data-node-url='" + nodeLink.Url + "']"];
                 Transform(element, nodeLink, cc, resource);
             });
        }

        public abstract void Transform(CQ element, NodeLink nodeLink, ControllerContext cc, INode resource);
    }
}