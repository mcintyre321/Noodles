using System.Linq;
using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class ApplyAttributeTransforms : IDocumentTransform
    {
        public void Register(ControllerContext cc, INode parent)
        {
            foreach (var att in parent.Attributes.OfType<INodeDocumentTransform>())
            {
                att.Transform(cc, parent);
            }
            foreach (var child in parent.ChildNodes.OfType<INode>())
            {
                foreach (var att in child.Attributes.OfType<IChildNodeDocumentTransform>())
                {
                    att.Transform(cc, child);
                }
            }
        }
    }

    public interface INodeDocumentTransform
    {
        void Transform(ControllerContext cc, INode node);
    }
    public interface IChildNodeDocumentTransform
    {
        void Transform(ControllerContext cc, INode child);
    }
}