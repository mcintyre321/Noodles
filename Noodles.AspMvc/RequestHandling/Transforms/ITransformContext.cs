using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public interface ITransformContext
    {
        void TransformContext(ControllerContext cc, INode parent);
    }
}