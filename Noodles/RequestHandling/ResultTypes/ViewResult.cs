using Noodles.Models;

namespace Noodles.RequestHandling.ResultTypes
{
    public class ViewResult : Result
    {
        public INode Node { get; set; }

        public ViewResult(INode node)
        {
            Node = node;
        }
    }
}