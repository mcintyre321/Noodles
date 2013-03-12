using Noodles.Models;

namespace Noodles.RequestHandling.ResultTypes
{
    public class ViewResult : Result
    {
        public INode Target { get; set; }

        public ViewResult(INode target)
        {
            Target = target;
        }
    }
}