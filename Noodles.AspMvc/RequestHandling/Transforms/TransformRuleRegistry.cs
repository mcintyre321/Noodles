using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Noodles.AspMvc.Infrastructure;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class TransformRuleRegistry
    {
        public List<ITransformContext> Transforms { get; private set; }
        public static TransformRuleRegistry Default { get; private set; }
        static TransformRuleRegistry()
        {
            Default = new TransformRuleRegistry()
            {
                Transforms =
                {
                    new ApplyButtonClass(),
                    new ApplyAttributeTransformsContext(),
                    new SelectFragmentTransformContext()
                }
            };
        }

        public TransformRuleRegistry()
        {
            Transforms = new List<ITransformContext>();
        }

        public void RegisterTransformations(ControllerContext cc, INode resource)
        {
            foreach (var documentTransform in Transforms)
            {
                documentTransform.TransformContext(cc, resource);
            }
        }

    }

    public class ApplyButtonClass : ITransformContext
    {
        public void TransformContext(ControllerContext cc, INode parent)
        {
            cc.HttpContext.Items.AddDocTransform(cq =>
            {
                cq[" input[type=submit]"].AddClass("btn").AddClass("btn-primary");
                cq[".node-component.node-method > a"].AddClass("btn").AddClass("btn-default");
            });
        }
    }
}