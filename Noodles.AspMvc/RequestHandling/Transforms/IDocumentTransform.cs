using System.Web.Mvc;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public interface IDocumentTransform
    {
        void Register(ControllerContext cc, INode parent);
    }
}