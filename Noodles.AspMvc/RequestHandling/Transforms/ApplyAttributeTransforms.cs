using System.Linq;
using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class ApplyAttributeTransforms : IDocumentTransform
    {
        public void Register(ControllerContext cc, INode resource)
        {
            foreach (var nodeLink in resource.ChildNodes)
            {
                foreach (var att in nodeLink.Attributes.OfType<ChildNodeDocumentTransformAttribute>())
                {
                    att.Transform(nodeLink, cc, resource);
                }
            }
        }
    }
}