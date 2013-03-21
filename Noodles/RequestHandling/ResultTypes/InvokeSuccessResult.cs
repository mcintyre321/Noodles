using Noodles.Models;

namespace Noodles.RequestHandling.ResultTypes
{
    public class InvokeSuccessResult : Result
    {
        private IInvokeable _invokeable;

        public InvokeSuccessResult(IInvokeable invokeable)
        {
            _invokeable = invokeable;
        }

        public IInvokeable Invokeable
        {
            get { return _invokeable; }
        }

        public object Result { get; set; }
    }
}