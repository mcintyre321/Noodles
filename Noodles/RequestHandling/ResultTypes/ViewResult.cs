using Noodles.Models;

namespace Noodles.RequestHandling.ResultTypes
{
    public class ViewResult : Result
    {
        public object Target { get; set; }

        public ViewResult(object target)
        {
            Target = target;
        }
    }
}