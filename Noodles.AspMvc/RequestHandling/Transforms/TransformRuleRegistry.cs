using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
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
}