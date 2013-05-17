using System.Collections.Generic;

namespace Noodles.RequestHandling.ResultTypes
{
    public class ErrorResult : Result
    {
        public ErrorResult()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; private set; }
    }
}