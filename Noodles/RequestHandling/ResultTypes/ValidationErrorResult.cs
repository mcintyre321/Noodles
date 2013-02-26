using Noodles.Models;

namespace Noodles.RequestHandling.ResultTypes
{
    public class ValidationErrorResult : Result
    {
        private IInvokeable _invokeable;

        public ValidationErrorResult(IInvokeable invokeable)
        {
            _invokeable = invokeable;
        }

        public IInvokeable Invokeable
        {
            get { return _invokeable; }
        }
    }
}