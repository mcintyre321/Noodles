namespace Noodles.Requests.Results
{
    public class InvokeSuccessResult : NoodlesResult
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
    }
}