using Noodles.Models;

namespace Noodles.Requests.Results
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