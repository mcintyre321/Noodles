namespace Noodles.Requests.Results
{
    public class NotFoundResult : NoodlesResult
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