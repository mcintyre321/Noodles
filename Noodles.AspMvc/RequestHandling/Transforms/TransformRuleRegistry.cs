using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
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
                    new ApplyButtonClass(),
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

    public class ApplyButtonClass : IDocumentTransform
    {
        public void Register(ControllerContext cc, INode resource)
        {
            cc.HttpContext.Items.AddTransform(cq =>
            {
                cq[" input[type=submit]"].AddClass("btn").AddClass("btn-primary");
                cq[".node-component.node-method > a"].AddClass("btn").AddClass("btn-default");
            });
        }
    }
}