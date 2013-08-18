using System.Linq;
using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
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
}