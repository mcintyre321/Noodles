using Noodles.Models;

namespace Noodles.Requests.Results
{
    public class NotFoundResult : Result
    {
        public INode FurthestNode { get; set; }
        public string Fragment { get; set; }

        public NotFoundResult(INode furthestNode, string fragment)
        {
            FurthestNode = furthestNode;
            Fragment = fragment;
        }
    }
}