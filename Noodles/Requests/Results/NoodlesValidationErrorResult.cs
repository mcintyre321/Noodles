using Noodles.Requests.Results;

namespace Noodles.Requests
{
    public class NoodlesValidationErrorResult : NoodlesResult
    {
        private IInvokeable _invokeable;

        public NoodlesValidationErrorResult(IInvokeable invokeable)
        {
            _invokeable = invokeable;
        }

        public IInvokeable Invokeable
        {
            get { return _invokeable; }
        }
    }
}