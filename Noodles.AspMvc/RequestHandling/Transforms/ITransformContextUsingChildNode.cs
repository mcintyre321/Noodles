using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public interface ITransformContextUsingChildNode
    {
        void Transform(ControllerContext cc, INode child);
    }
}