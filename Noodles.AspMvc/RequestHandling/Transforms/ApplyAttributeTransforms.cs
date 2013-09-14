using System.Linq;
using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class ApplyAttributeTransformsContext : ITransformContext
    {
        public void TransformContext(ControllerContext cc, INode parent)
        {
            foreach (var transform in parent.Attributes.OfType<ITransformContext>())
            {
                transform.TransformContext(cc, parent);
            }
            foreach (var child in parent.ChildNodes.OfType<INode>())
            {
                foreach (var att in child.Attributes.OfType<ITransformContextUsingChildNode>())
                {
                    att.Transform(cc, child);
                }
            }
        }
    }
}