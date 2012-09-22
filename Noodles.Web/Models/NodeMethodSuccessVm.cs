namespace Noodles.Web.Models
{
    public class NodeMethodSuccessVm
    {
        public NodeMethod Method { get; set; }
        public object Result { get; set; }

        public NodeMethodSuccessVm(NodeMethod method, object result)
        {
            Method = method;
            Result = result;
        }
    }
}