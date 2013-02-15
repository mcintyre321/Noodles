using Noodles.Requests.Results;

namespace Noodles.Requests
{
    public class NoodlesViewResult : NoodlesResult
    {
        public INode Node { get; set; }

        public NoodlesViewResult(INode node)
        {
            Node = node;
        }
    }
}